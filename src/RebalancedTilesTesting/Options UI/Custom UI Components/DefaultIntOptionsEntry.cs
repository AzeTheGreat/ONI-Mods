using PeterHan.PLib;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System;
using TMPro;
using UnityEngine;

namespace RebalancedTilesTesting.CustomUIComponents
{
	// Largely taken from Peter's IntOptionsEntry - modified to suit these specific needs.
	public class DefaultIntOptionsEntry : OptionsEntry
    {
		public override object Value
        {
			// TODO: convert just when used?  Add secondary getter to prevent needing to?
			get => Convert.ToInt32(Options.Opts.configOptions.GetModifierValue(defId, propertyId));
            set
            {
                Options.Opts.configOptions.SetModifierValue(defId, propertyId, value);
                Update();
            }
        }

		private readonly LimitAttribute limits;
        private readonly int defaultValue;
		private readonly string defId;
		private readonly string propertyId;
		private GameObject textField;

		public DefaultIntOptionsEntry(string title, string tooltip, int defaultValue, string defId, string propertyId, string category = "", LimitAttribute limits = null) : base(title, tooltip, category)
		{
			textField = null;
			this.defaultValue = defaultValue;
			this.defId = defId;
			this.propertyId = propertyId;
			this.limits = limits;
		}

        public override GameObject GetUIComponent()
		{
			textField = new PTextField()
			{
				OnTextChanged = OnTextChanged,
				ToolTip = ToolTip,
				Text = Value.ToString(),
				MinWidth = 64,
				MaxLength = 10,
				Type = PTextField.FieldType.Integer
			}
			.Build();

			Update();
			return textField;
		}

		private void OnTextChanged(GameObject _, string text)
		{
			if (int.TryParse(text, out int newValue))
			{
				if (limits != null)
					newValue = limits.ClampToRange(newValue);
				Value = newValue;
			}
			else
				Update();
		}

		protected void Update()
		{
			var field = textField?.GetComponentInChildren<TMP_InputField>();

			if (field != null)
            {
				field.text = ((int)Value).ToString(Format ?? "D");
				field.textComponent.color = (int)Value == defaultValue ? Color.gray : Color.black;
            }
		}
	}
}
