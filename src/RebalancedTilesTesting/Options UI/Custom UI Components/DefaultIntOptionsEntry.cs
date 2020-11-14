using PeterHan.PLib;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System;
using TMPro;
using UnityEngine;

namespace RebalancedTilesTesting.CustomUIComponents
{
    // Largely taken from Peter's IntOptionsEntry - modified to suit these specific needs.
    public class DefaultIntOptionsEntry : SlidingBaseOptionsEntry
    {
		public override object Value
        {
			// TODO: convert just when used?  Add secondary getter to prevent needing to?
            get => Options.Opts.configOptions.TryGetValue(defId, propertyId, out var value) ? Convert.ToInt32(value) : defaultValue;
            set
            {
                Options.Opts.configOptions.SetValue(defId, propertyId, (int)value == defaultValue ? null : value);
                Update();
            }
        }

        private readonly int defaultValue;
		private readonly string defId;
		private readonly string propertyId;
		private GameObject textField;

		public DefaultIntOptionsEntry(string title, string tooltip, int defaultValue, string defId, string propertyId, string category = "", LimitAttribute limits = null) : base(title, tooltip, category, limits)
		{
			textField = null;
			this.defaultValue = defaultValue;
			this.defId = defId;
			this.propertyId = propertyId;
		}

		protected override PSliderSingle GetSlider()
		{
			return null;
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
			}.Build();

			Update();
			return textField;
		}

		public void EnableOption(bool enable)
        {
			textField.GetParent().SetActive(enable);
        }

		private void OnSliderChanged(GameObject _, float newValue)
		{
			int newIntValue = Mathf.RoundToInt(newValue);
			if (limits != null)
				newIntValue = limits.ClampToRange(newIntValue);
			Value = newIntValue;
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

		protected override void Update()
		{
			var field = textField?.GetComponentInChildren<TMP_InputField>();

			if (field != null)
            {
				field.text = ((int)Value).ToString(Format ?? "D");
				field.textComponent.color = (int)Value == defaultValue ? Color.gray : Color.black;
            }
			if (slider != null)
				PSliderSingle.SetCurrentValue(slider, (float)Value);
		}
	}
}
