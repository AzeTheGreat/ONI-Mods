using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModManager
{
    public class SearchUI : UISource
    {
        protected override IUIComponent GetUIComponent()
        {
            var tf = new PTextField()
            {
                Text = "Search for mods",
                FlexSize = Vector2.one
            }
            // PTextField's OnTextChanged only triggers when the user hits enter.
            // TMP's triggers on every character change, giving a more responsive search.
            .AddOnRealize(AddTextFieldListener);

            return new PPanel()
            {
                Margin = new(2, 2, 2, 2),
                FlexSize = Vector2.one
            }
            .AddChild(tf);

            void AddTextFieldListener(GameObject go)
            {
                var textField = go.GetComponent<TMP_InputField>();
                textField.onValueChanged.AddListener(
                    t => AExecuteEvents.ExecuteOnEntireHierarchy<ITextChanged>(go, x => x.OnSeachFieldChanged(t)));
            }
        }

        public interface ITextChanged : IEventSystemHandler
        {
            void OnSeachFieldChanged(string text);
        }
    }
}
