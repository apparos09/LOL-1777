using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_MST
{
    // The puzzle conversion display.
    public class PuzzleConversionDisplay : MonoBehaviour
    {
        // The symbol image.
        public Image symbolImage;

        // The conversion text.
        public TMP_Text measurementValueText;

        // The multiplier text.
        public TMP_Text conversionMultipleText;

        // The units button this conversion display corresponds to.
        public UnitsButton unitsButton;

        // Sets the symbol sprite.
        public void SetSymbolSprite(Sprite symbolSprite)
        {
            symbolImage.sprite = symbolSprite;
        }

        // Sets the text using a units button.
        public void SetTextFromUnitsButton(UnitsButton unitsButton)
        {
            this.unitsButton = unitsButton;
            measurementValueText.text = unitsButton.measurementValueText.text;
            conversionMultipleText.text = unitsButton.conversionMultipleText.text;
        }

        // Clears all elements.
        public void Clear()
        {
            symbolImage.sprite = null;
            measurementValueText.text = "-";
            conversionMultipleText.text = "-";
        }
    }
}
