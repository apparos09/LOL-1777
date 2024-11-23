using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static RM_MST.PuzzleManager;

namespace RM_MST
{
    // The puzzle UI.
    public class PuzzleUI : MonoBehaviour
    {
        // the instance of the class.
        private static PuzzleUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The stage UI.
        public StageUI stageUI;

        // The puzzle manager.
        public PuzzleManager puzzleManager;

        [Header("Unit Buttons")]
        // The units buttons.
        public RectTransform unitButtonsParent;

        // The default position of the unit buttons object.
        private Vector3 unitButtonsDefaultPos;

        // The unit buttons hide object (used to hide the buttons off-screen).
        public RectTransform unitButtonsHiddenParent;

        [Header("Puzzle")]

        // The conversion displays.
        public GameObject conversionDisplays;

        // The puzzle window.
        public GameObject puzzleWindow;

        // Gets set to 'true' when late start has been called.
        private bool calledLateStart = false;

        // Constructor
        private PuzzleUI()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;

                // Saves the default position.
                unitButtonsDefaultPos = unitButtonsParent.anchoredPosition;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Sets the stage UI.
            if (stageUI == null)
                stageUI = StageUI.Instance;

            // Sets the puzzle manager.
            if (puzzleManager == null)
                puzzleManager = PuzzleManager.Instance;
        }

        // Called on the first update frame of the puzzle UI.
        private void LateStart()
        {
            calledLateStart = true;
        }

        // Gets the instance.
        public static PuzzleUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<PuzzleUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("PuzzleUI (singleton)");
                        instance = go.AddComponent<PuzzleUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Resets the positions of the unit buttons parent object.
        public void ResetUnitButtonsParentPosition()
        {
            unitButtonsParent.anchoredPosition = unitButtonsDefaultPos;
        }

        // Move the unit buttons parent to the hidden position.
        public void MoveUnitButtonsParentToHiddenPosition()
        {
            unitButtonsParent.anchoredPosition = unitButtonsHiddenParent.anchoredPosition;
        }

        // Called when a puzzle has been generated.
        public void OnPuzzleGenerated()
        {
            // Generate the puzzle.
            switch (puzzleManager.pType)
            {
                // Show the unit buttons, and hide the puzzle UI.
                case puzzleType.none:
                case puzzleType.buttons:
                    
                    unitButtonsParent.gameObject.SetActive(true);
                    ResetUnitButtonsParentPosition();

                    puzzleWindow.SetActive(false);
                    conversionDisplays.gameObject.SetActive(false);

                    break;

                    // Hide the unit buttons and show the puzzle UI.
                case puzzleType.swap:
                case puzzleType.slide:
                case puzzleType.path:

                    unitButtonsParent.gameObject.SetActive(false);
                    MoveUnitButtonsParentToHiddenPosition();

                    puzzleWindow.gameObject.SetActive(true);
                    conversionDisplays.gameObject.SetActive(true);
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Calls LateStart.
            if (!calledLateStart)
                LateStart();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}