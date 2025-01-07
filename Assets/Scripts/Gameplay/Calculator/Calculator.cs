using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_MST
{
    // An in-game calculator.
    public class Calculator : MonoBehaviour
    {
        // The text for the calculator display.
        public TMP_Text displayText;

        // The keys for the calculator.
        public List<CalculatorKey> keys = new List<CalculatorKey>();

        // Used to see if the calculator is interactable.
        private bool interactable = true;

        // Start is called before the first frame update
        void Start()
        {
            // If there are no saved keys, auto-set.
            if(keys.Count == 0)
            {
                keys = new List<CalculatorKey>(GetComponentsInChildren<CalculatorKey>());
            }

            // Clear the calculator to start.
            Clear();
        }

        // Adds the key's character.
        public void AddCharacterToEquation(char keyChar)
        {
            // Checks what kind of character has been sent.
            switch(keyChar)
            {
                case 'C': // "C" means Clear
                case 'c':
                    Clear();
                    break;

                case '=': // "=" means Solve
                    TrySolve();
                    break;

                default:
                    displayText.text += keyChar;
                    break;
            }
        }

        // Tries to solve the equation put into the calculator.
        // The result is saved in the display text.
        public bool TrySolve()
        {
            Debug.Log("TODO: Implement Solve!");
            return false;
        }

        // Clears the equation.
        public void Clear()
        {
            displayText.text = "";
        }

        // Sets if the calculator is interactable or not.
        public void SetCalculatorInteractable(bool interactable)
        {
            // Save the value.
            this.interactable = interactable;

            // TODO: change the display text? Maybe change the text colour, or leave it as is?

            // Goes through all the keys.
            foreach(CalculatorKey key in keys)
            {
                // Change interactable of all buttons.
                key.button.interactable = interactable;
            }
        }
    }
}