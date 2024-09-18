using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_MST
{
    // A units button.
    public class UnitsButton : MonoBehaviour
    {
        // The button;
        public Button button;

        // The button's text.
        public TMP_Text text;

        // The measurement value.
        private float measurementValue = 0;

        // The symbol for the button.
        private string unitsSymbol = string.Empty;

        // Start is called before the first frame update
        void Start()
        {
            // Autoset the button.
            if(button == null)
                button = GetComponent<Button>();

            // Autoset the text.
            if(text == null)
                text = GetComponentInChildren<TMP_Text>();
        }

        // Returns the measurement value.
        public float GetMeasurementValue()
        {
            return measurementValue;
        }

        // Sets the text with a value and symbol.
        public void SetMeasurementValue(float value)
        {
            measurementValue = value;
            UpdateText();
        }

        // Sets the text with a value and symbol.
        public void SetMeasurementValueAndSymbol(float value, string symbol)
        {
            measurementValue = value;
            unitsSymbol = symbol;
            UpdateText();
        }

        // Sets text using the provided conversion.
        public void SetMeasurementValueAndSymbol(UnitsInfo.UnitsConversion conversion)
        {
            SetMeasurementValueAndSymbol(conversion.GetConvertedValue(), conversion.GetOutputSymbol());
        }

        // Returns the unit symbol.
        public string GetUnitSymbol()
        {
            return unitsSymbol;
        }

        // Updates the text.
        public void UpdateText()
        {
            text.text = measurementValue.ToString() + " " + unitsSymbol;
        }

        // Clears the button.
        public void ClearButton()
        {
            measurementValue = 0;
            unitsSymbol = string.Empty;
            text.text = "-";
        }

    }
}
