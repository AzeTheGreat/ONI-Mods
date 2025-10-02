using System;
using System.Collections.Generic;
using System.Reflection;
using AzeLib.Attributes;
using Xunit;

namespace AzeLib.Tests.Attributes
{
    public class AMonoBehaviourTests
    {
        private sealed class FakeComponent { }

        private sealed class AnotherComponent { }

        private sealed class TestMonoBehaviour : AMonoBehaviour
        {
            private readonly Dictionary<Type, object> _components;

            public TestMonoBehaviour(Dictionary<Type, object> components)
            {
                _components = components;
            }

            [MyIntGet]
            private FakeComponent? _fakeComponent;

            [MyIntGet]
            private AnotherComponent? _anotherComponent;

            public FakeComponent? FakeComponent => _fakeComponent;

            public AnotherComponent? AnotherComponent => _anotherComponent;

            protected override object? ResolveComponent(Type componentType)
            {
                _components.TryGetValue(componentType, out var component);
                return component;
            }
        }

        [Fact]
        public void OnSpawn_AssignsComponentsToDecoratedFields()
        {
            var fake = new FakeComponent();
            var another = new AnotherComponent();
            var componentMap = new Dictionary<Type, object>
            {
                [typeof(FakeComponent)] = fake,
                [typeof(AnotherComponent)] = another
            };

            var mono = new TestMonoBehaviour(componentMap);

            var resolved = new List<(FieldInfo Field, object? Value)>();
            AMonoBehaviour.DebugHooks.ComponentResolved = (instance, field, value) =>
            {
                if (ReferenceEquals(instance, mono))
                {
                    resolved.Add((field, value));
                }
            };

            try
            {
                mono.OnSpawn();

                Assert.Same(fake, mono.FakeComponent);
                Assert.Same(another, mono.AnotherComponent);
                Assert.Equal(2, resolved.Count);
                Assert.Contains(resolved, entry => entry.Field.Name == "_fakeComponent" && ReferenceEquals(entry.Value, fake));
                Assert.Contains(resolved, entry => entry.Field.Name == "_anotherComponent" && ReferenceEquals(entry.Value, another));
            }
            finally
            {
                AMonoBehaviour.DebugHooks.ComponentResolved = null;
            }
        }

        [Fact]
        public void OnSpawn_PrimesFieldCacheOncePerType()
        {
            var componentMap = new Dictionary<Type, object>();
            var cacheHits = 0;

            AMonoBehaviour.DebugHooks.CachePrimed = (type, _) =>
            {
                if (type == typeof(TestMonoBehaviour))
                {
                    cacheHits++;
                }
            };

            try
            {
                var first = new TestMonoBehaviour(componentMap);
                first.OnSpawn();

                var second = new TestMonoBehaviour(componentMap);
                second.OnSpawn();

                Assert.Equal(1, cacheHits);
            }
            finally
            {
                AMonoBehaviour.DebugHooks.CachePrimed = null;
            }
        }
    }
}
