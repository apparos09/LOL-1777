using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The puzzle piece that's used for the game.
    public class PuzzlePiece : MonoBehaviour
    {
        // The conversion display this puzzle piece is connected to.
        public PuzzleConversionDisplay conversionDisplay;

        // Sets the display info on start if there is a display set.
        public bool setDisplayInfoOnStart = true;

        [Header("Sprites")]

        // The symbol sprite.
        public SpriteRenderer symbolSpriteRenderer;

        // The main piece sprite.
        public SpriteRenderer pieceSpriteRenderer;

        // Start is called before the first frame update
        void Start()
        {
            // Checks if the information should be set on start.
            if(setDisplayInfoOnStart)
            {
                SetPieceFromConversionDisplay(conversionDisplay);
            }
        }

        // Sets the values from the conversion display.
        public void SetPieceFromConversionDisplay()
        {
            // Old - copies sprite from display to piece.
            symbolSpriteRenderer.sprite = conversionDisplay.symbolImage.sprite;

            // Get the colour from the units button if it exists.
            if(conversionDisplay.unitsButton != null)
            {
                pieceSpriteRenderer.color = conversionDisplay.unitsButton.button.image.color;
            }
        }

        // Sets the values from the conversion display.
        public void SetPieceFromConversionDisplay(PuzzleConversionDisplay newDisplay)
        {
            conversionDisplay = newDisplay;
            SetPieceFromConversionDisplay();
        }

        // Called when the puzzle piece has been selected.
        public virtual void OnSelect()
        {
            // Trigger the unit button.
            // TODO: check if the button is interactable?
            conversionDisplay.unitsButton.button.onClick.Invoke();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}