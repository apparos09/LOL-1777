using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using util;

namespace RM_MST
{
    // Used to manage debug operations to test for puzzles.
    public class PuzzleManagerDebug : MonoBehaviour
    {
        // The mouse touch input object.
        public util.MouseTouchInput mouseTouchInput;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            // If the pointer is over a UI element.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Pointer Over UI Object. Mouse Pos in Screen: " + MouseTouchInput.GetMousePositionInScreenSpace());

            }
        }
    }
}