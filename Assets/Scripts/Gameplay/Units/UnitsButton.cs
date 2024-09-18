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
        // The stage manager.
        public StageManager stageManager;

        // The button;
        public Button button;

        // The button's text.
        public TMP_Text text;

        // The measurement value.
        private float measurementValue = 0;

        // The symbol for the button.
        private string unitsSymbol = string.Empty;

        // Gets set to 'true' when this is the correct value.
        // This is an alternate way to see if this button is the correct one.
        [Tooltip("An alternate way to check that this button is correct. Call related function to auto set.")]
        public bool correctValue = false;

        // Start is called before the first frame update
        void Start()
        {
            // Grab the instance.
            if (stageManager == null)
                stageManager = StageManager.Instance;

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
            correctValue = false;
        }

        // Clears the button.
        public void ClearButton()
        {
            measurementValue = 0;
            unitsSymbol = string.Empty;
            text.text = "-";
            correctValue = false;
        }

        // Checks and returns if this is the correct value.
        public bool IsCorrectValue()
        {
            // Sets the instance.
            if(stageManager == null)
                stageManager = StageManager.Instance;

            // False by default.
            correctValue = false;

            // If the targeter has a meteor.
            if (stageManager.meteorTarget.meteor != null)
            {
                // Values match, meaning this is the correct value.
                if(stageManager.meteorTarget.meteor.GetConvertedValue() == measurementValue)
                {
                    correctValue = true;
                }
            }          

            // Return result.
            return correctValue;
        }

    }
}
