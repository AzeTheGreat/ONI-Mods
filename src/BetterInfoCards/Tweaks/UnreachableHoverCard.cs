using AzeLib.Extensions;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    public static class DetectRunStart_Patch
    {
        private static readonly Func<SelectToolHoverTextCard, object> hoverObjectsGetter = CreateHoverObjectsGetter();
        private static readonly Dictionary<Type, Func<object, KSelectable>> hoverObjectExtractors = new();
        private static readonly object hoverObjectExtractorLock = new();

        public static IEnumerable<CodeInstruction> ChildTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo targetMethod = AccessTools.Method(typeof(Component), nameof(Component.GetComponent)).MakeGenericMethod(typeof(ChoreConsumer));

            bool afterTarget = false;

            foreach (CodeInstruction i in instructions)
            {
                if (i.Is(OpCodes.Callvirt, targetMethod))
                    afterTarget = true;

                if (afterTarget && i.OpCodeIs(OpCodes.Ldc_I4_0))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0) { labels = new List<Label>(i.labels) };
                    i.labels.Clear();

                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DetectRunStart_Patch), nameof(GetHoverObjects)));

                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DetectRunStart_Patch), nameof(DrawUnreachableCard)));
                    afterTarget = false;
                }

                yield return i;
            }
        }

        private static IEnumerable<KSelectable> GetHoverObjects(SelectToolHoverTextCard instance) => EnumerateSelectables(hoverObjectsGetter(instance));

        private static void DrawUnreachableCard(SelectToolHoverTextCard instance, IEnumerable<KSelectable> hoverObjects)
        {
            var unreachable = Db.Get().MiscStatusItems.PickupableUnreachable;

            if (hoverObjects.Any(x => x.HasStatusItem(unreachable)))
            {
                HoverTextDrawer drawer = HoverTextScreen.Instance.drawer;

                drawer.BeginShadowBar();
                drawer.DrawIcon(unreachable.sprite.sprite, instance.Styles_BodyText.Standard.textColor, 18, -6);
                drawer.AddIndent(8);
                drawer.DrawText(unreachable.Name.ToUpper(), instance.Styles_Title.Standard);
                drawer.EndShadowBar();
            }
        }

        private static Func<SelectToolHoverTextCard, object> CreateHoverObjectsGetter()
        {
            foreach (string memberName in new[] { "hoverObjects", "overlayValidHoverObjects" })
            {
                if (AccessTools.DeclaredField(typeof(SelectToolHoverTextCard), memberName) is FieldInfo field)
                    return instance => field.GetValue(instance);

                if (AccessTools.DeclaredProperty(typeof(SelectToolHoverTextCard), memberName) is PropertyInfo property)
                    return instance => property.GetValue(instance);
            }

            return _ => null;
        }

        private static IEnumerable<KSelectable> EnumerateSelectables(object value)
        {
            if (value == null)
                yield break;

            if (value is IEnumerable<KSelectable> directEnumerable)
            {
                foreach (var selectable in directEnumerable)
                {
                    if (selectable != null)
                        yield return selectable;
                }

                yield break;
            }

            if (value is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    if (TryGetSelectable(item, out var selectable))
                        yield return selectable;
                }

                yield break;
            }

            if (TryGetSelectable(value, out var single))
                yield return single;
        }

        private static bool TryGetSelectable(object value, out KSelectable selectable)
        {
            switch (value)
            {
                case null:
                    selectable = null;
                    return false;
                case KSelectable ks:
                    selectable = ks;
                    return true;
                case Component component:
                    selectable = component.GetComponent<KSelectable>();
                    return selectable != null;
                case GameObject go:
                    selectable = go.GetComponent<KSelectable>();
                    return selectable != null;
            }

            var type = value.GetType();

            Func<object, KSelectable> extractor;
            lock (hoverObjectExtractorLock)
            {
                if (!hoverObjectExtractors.TryGetValue(type, out extractor))
                {
                    extractor = CreateExtractor(type);
                    hoverObjectExtractors[type] = extractor;
                }
            }

            if (extractor == null)
            {
                selectable = null;
                return false;
            }

            selectable = extractor(value);
            return selectable != null;
        }

        private static Func<object, KSelectable> CreateExtractor(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            FieldInfo selectableField = type.GetFields(flags)
                .FirstOrDefault(f => typeof(KSelectable).IsAssignableFrom(f.FieldType));
            if (selectableField != null)
                return obj => selectableField.GetValue(obj) as KSelectable;

            PropertyInfo selectableProperty = type.GetProperties(flags)
                .FirstOrDefault(p => p.CanRead && p.GetIndexParameters().Length == 0 && typeof(KSelectable).IsAssignableFrom(p.PropertyType));
            if (selectableProperty != null)
                return obj => selectableProperty.GetValue(obj) as KSelectable;

            FieldInfo componentField = type.GetFields(flags)
                .FirstOrDefault(f => typeof(Component).IsAssignableFrom(f.FieldType));
            if (componentField != null)
                return obj => (componentField.GetValue(obj) as Component)?.GetComponent<KSelectable>();

            PropertyInfo componentProperty = type.GetProperties(flags)
                .FirstOrDefault(p => p.CanRead && p.GetIndexParameters().Length == 0 && typeof(Component).IsAssignableFrom(p.PropertyType));
            if (componentProperty != null)
                return obj => (componentProperty.GetValue(obj) as Component)?.GetComponent<KSelectable>();

            FieldInfo goField = type.GetFields(flags)
                .FirstOrDefault(f => typeof(GameObject).IsAssignableFrom(f.FieldType));
            if (goField != null)
                return obj => (goField.GetValue(obj) as GameObject)?.GetComponent<KSelectable>();

            PropertyInfo goProperty = type.GetProperties(flags)
                .FirstOrDefault(p => p.CanRead && p.GetIndexParameters().Length == 0 && typeof(GameObject).IsAssignableFrom(p.PropertyType));
            if (goProperty != null)
                return obj => (goProperty.GetValue(obj) as GameObject)?.GetComponent<KSelectable>();

            return null;
        }
    }
}
