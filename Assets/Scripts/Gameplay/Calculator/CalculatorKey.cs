using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_MST
{
    // A key for the calculator.
    public class CalculatorKey : MonoBehaviour
    {
        // The calculator this key is attached to.
        public Calculator calculator;

        // The key text.
        public TMP_Text keyText;

        // The key character. This is used to determine what key is provided.
        public char keyChar;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Called when the key has been pressed.
        public virtual void OnKeyPressed()
        {
            calculator.AddKeyCharacter(keyChar);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}