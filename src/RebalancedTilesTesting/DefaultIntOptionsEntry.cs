using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;

namespace RebalancedTilesTesting
{
    // Largely taken from Peter's IntOptionsEntry - modified to suit these specific needs.
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultIntOptionsEntry : SlidingBaseOptionsEntry
    {
		public override object Value
		{
			get => savedValue == null ? defaultValue : savedValue;
			set
			{
				if (value is int newValue)
				{
					savedValue = newValue;
					if (savedValue == defaultValue)
						savedValue = null;
				}
				Update();
			}
		}

		[JsonProperty] public int? savedValue;
		private readonly int defaultValue;

		private GameObject textField;

		public DefaultIntOptionsEntry(string title, string tooltip, int defaultValue, string category = "", DefaultIntOptionsEntry lastOption = null, LimitAttribute limits = null) : base(title, tooltip, category, limits)
		{
			textField = null;
			this.defaultValue = defaultValue;
			Value = lastOption?.savedValue;
		}

		protected override PSliderSingle GetSlider()
		{
			return new PSliderSingle()
			{
				OnValueChanged = OnSliderChanged,
				ToolTip = ToolTip,
				MinValue = (float)limits.Minimum,
				MaxValue = (float)limits.Maximum,
				InitialValue = (float)Value,
				IntegersOnly = true
			};
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
				field.textComponent.color = savedValue == null ? Color.gray : Color.black;
            }
			if (slider != null)
				PSliderSingle.SetCurrentValue(slider, (float)Value);
		}
	}
}
