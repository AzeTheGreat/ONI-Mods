using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ModManager
{
    public class SearchUI : IUISource
    {
        public UnityAction<string> OnTextChanged { get; set; }

        public IUIComponent GetUIComponent()
        {
            var tf = new PTextField()
            {
                Text = "Search for mods",
                FlexSize = Vector2.one
            }
            // PTextField's OnTextChanged only triggers when the user hits enter.
            // TMP's triggers on every character change, giving a more responsive search.
            .AddOnRealize((GameObject realized) =>
            {
                if(OnTextChanged != null)
                {
                    var textField = realized.GetComponent<TMP_InputField>();
                    textField.onValueChanged.AddListener(OnTextChanged);
                }
            });

            return new PPanel()
            {
                Margin = new(2, 2, 2, 2),
                FlexSize = Vector2.one
            }
            .AddChild(tf);
        }
    }
}
