using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The puzzle piece that's used for the game.
    public class PuzzlePiece : MonoBehaviour
    {
        // The units button this puzzle piece is attached to.
        public UnitsButton unitsButton;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Called when the puzzle piece has been selected.
        public void OnSelect()
        {
            // Trigger the unit button.
            // TODO: check if the button is interactable?
            unitsButton.button.onClick.Invoke();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}