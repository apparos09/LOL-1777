using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using util;

namespace RM_MST
{
    // A units button.
    public class UnitsButton : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The button;
        public Button button;

        // The button's output (result) text.
        public TMP_Text measurementValueText;

        // The button's conversion text.
        public TMP_Text conversionMultipleText;

        // The measurement value.
        private float measurementValue = 0;

        // The conversion multiple.
        private float conversionMultiple = 0;

        // The symbol for the button.
        private string unitsSymbol = string.Empty;

        // The colour for the laser.
        [Tooltip("The colour of the laser that's shot by the units button.")]
        public Color laserColor = Color.white;

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
            if(measurementValueText == null)
                measurementValueText = GetComponentInChildren<TMP_Text>();
        }

        // Returns the measurement value.
        public float GetMeasurementValue()
        {
            return measurementValue;
        }

        // Gets the multiple that resulted in the measurement value.
        public float GetConversionMultiple()
        {
            return conversionMultiple;
        }

        // Gets the conversion multiple rounded.
        public float GetConversionMultipleRounded()
        {
            return CustomMath.Round(conversionMultiple, StageManager.UNITS_DECIMAL_PLACES);
        }

        // Gets the conversion multiple rounded.
        public float GetConversionMultipleRounded(int decimalPlaces)
        {
            return CustomMath.Round(conversionMultiple, decimalPlaces);
        }


        // Returns the unit symbol.
        public string GetUnitSymbol()
        {
            return unitsSymbol;
        }

        // Sets the text with a value and symbol.
        public void SetMeasurementValueAndSymbol(float measurement, float conversionMult, string symbol)
        {
            measurementValue = measurement;
            conversionMultiple = conversionMult;
            unitsSymbol = symbol;
            UpdateText();
        }

        // Sets the measurement value and symbol.
        // If 'showFraction' is 'true', the value is shown as a fraction if it is less than 1.
        public void SetMeasurementValueAndSymbol(float measurement, float conversionMult, string symbol, bool showFraction)
        {
            measurementValue = measurement;
            conversionMultiple = conversionMult;
            unitsSymbol = symbol;
            UpdateText(showFraction);
        }

        // Sets text using the provided conversion.
        public void SetMeasurementValueAndSymbol(UnitsInfo.UnitsConversion conversion, bool showFraction)
        {
            // Set the measurement value.
            SetMeasurementValueAndSymbol(conversion.GetConvertedValue(), conversion.CalculateConversionMultiplier(), conversion.GetOutputSymbol(), showFraction);
        }

        // Gets the conversion multiple's display string.
        private string GetConversionMultipleDisplayString()
        {
            return "x" + GetConversionMultipleRounded().ToString();
        }

        // Updates the text.
        public void UpdateText()
        {
            measurementValueText.text = measurementValue.ToString() + " " + unitsSymbol;
            conversionMultipleText.text = GetConversionMultipleDisplayString();
            correctValue = false;
        }

        // Updates the text.
        public void UpdateText(bool showFraction)
        {
            // Checks if the text has been updated.
            bool textUpdated = false;

            // If a fraction should be shown.
            if(showFraction)
            {
                // If the measurement value is in a 0-1 range.
                if(measurementValue > 0.0F && measurementValue < 1.0F)
                {
                    // The measurement as a string, and the number of decimal places.
                    string measurementStr = measurementValue.ToString();
                    int decimalPlaces = 0;

                    // If there is a decimal place, use it to get the amount of decimal places.
                    if (measurementStr.Contains("."))
                    {
                        // Calculates the number of decimal places to know what to display.
                        decimalPlaces = measurementStr.Length - (measurementStr.IndexOf(".") + 1);
                    }

                    // There are decimal places, generate the result string.
                    if (decimalPlaces > 0)
                    {
                        // Sets up the text string and sets it.
                        float mult = Mathf.Pow(10, decimalPlaces);
                        string textStr = (measurementValue * mult).ToString() + "/" + mult.ToString() + " " + unitsSymbol;
                        measurementValueText.text = textStr;

                        // The text has been updated.
                        textUpdated = true;
                    }
                }

            }

            // If the text wasn't updated, do a normal update.
            if(textUpdated)
            {
                conversionMultipleText.text = GetConversionMultipleDisplayString();
                correctValue = false;
            }
            else
            {
                UpdateText();
            }
        }


        // Clears the button.
        public void ClearButton()
        {
            measurementValue = 0;
            conversionMultiple = 0;
            unitsSymbol = string.Empty;

            measurementValueText.text = "-";
            conversionMultipleText.text = "-";

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

            // Gets the target meteor.
            Meteor targetMeteor = stageManager.meteorTarget.GetMeteor();

            // If the targeter has a meteor.
            if (targetMeteor != null)
            {
                // Values match, meaning this is the correct value.
                if(targetMeteor.GetConvertedValue() == measurementValue)
                {
                    correctValue = true;
                }
            }          

            // Return result.
            return correctValue;
        }

    }
}
