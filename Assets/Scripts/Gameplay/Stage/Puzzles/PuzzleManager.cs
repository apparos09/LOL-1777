using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RM_MST
{
    // The manager for the puzzles.
    public class PuzzleManager : MonoBehaviour
    {
        // The puzzle types.
        public enum puzzleType { none, buttons, swap, slide, path }

        // the instance of the class.
        private static PuzzleManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The stage manager.
        public StageManager stageManager;

        // The puzzle UI.
        public PuzzleUI puzzleUI;

        // Gets set to 'true' when late start has been called.
        private bool calledLateStart = false;

        [Header("Puzzles")]

        // The type of puzzle being generated.
        public puzzleType pType = puzzleType.none;

        // The puzzle being used.
        public Puzzle puzzle;

        // The parent object for the puzzle.
        public GameObject puzzleParent;

        [Header("Puzzles/Prefabs")]

        // The swap puzzle prefab.
        public SwapPuzzle swapPuzzlePrefab;

        // The slide puzzle prefab.
        public SlidePuzzle slidePuzzlePrefab;

        // The path puzzle prefab.
        public PathPuzzle pathPuzzlePrefab;

        // TODO: puzzle prefabs.

        // Constructor
        private PuzzleManager()
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
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the stage manager.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // Gets the puzzle UI.
            if (puzzleUI == null)
                puzzleUI = PuzzleUI.Instance;
        }

        // Called on the first update frame of the puzzle UI.
        private void LateStart()
        {
            calledLateStart = true;

            // Generates the puzzle.
            GeneratePuzzle();
        }

        // Gets the instance.
        public static PuzzleManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<PuzzleManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("PuzzleManager (singleton)");
                        instance = go.AddComponent<PuzzleManager>();
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

        // Generates the puzzle with the set type.
        public void GeneratePuzzle()
        {
            // If the puzzle is set.
            if (puzzle != null)
            {
                // Destroy the object.
                Destroy(puzzle.gameObject);
            }

            // Generate the puzzle.
            switch (pType)
            {
                // The none and buttons puzzle type produce the same result.
                case puzzleType.none:
                case puzzleType.buttons:
                    // No changes.
                    break;

                case puzzleType.swap:
                    if(swapPuzzlePrefab != null)
                        puzzle = Instantiate(swapPuzzlePrefab);
                    break;

                case puzzleType.slide:
                    if (slidePuzzlePrefab != null)
                        puzzle = Instantiate(slidePuzzlePrefab);
                    break;

                case puzzleType.path:
                    if (pathPuzzlePrefab != null)
                        puzzle = Instantiate(pathPuzzlePrefab);
                    break;
            }

            // The puzzle type is not set, meaning nothing was instantiated.
            if(puzzle == null)
            {
                // The puzzle tpye is not set to none or buttons.
                if(pType != puzzleType.none && pType != puzzleType.buttons)
                {
                    Debug.LogError("Puzzle was not instantiated. The puzzleType was reset to 'none'.");
                    pType = puzzleType.none;
                }                
            }
            else
            {
                // Set the puzzle parent and reset the local position.
                puzzle.transform.parent = puzzleParent.transform;
                puzzle.transform.localPosition = Vector3.zero;
            }

            // The puzzle has been generated.
            puzzleUI.OnPuzzleGenerated();

        }

        // Generates the puzzle with the provided type.
        public void GeneratePuzzle(puzzleType newType)
        {
            // Set the type.
            pType = newType;

            // Generate the puzzle.
            GeneratePuzzle();
        }

        // Called when a meteor has been targeted.
        public void OnMeteorTargeted(Meteor meteor)
        {
            // Refreshes the conversion displays.
            puzzleUI.RefreshConversionDisplays();

            // Starts the puzzle.
            if(puzzle != null)
            {
                puzzle.StartPuzzle();
            }
        }

        // Called when the stage has ended.
        public void OnStageEnd()
        {
            // If there is a puzzle, end it and put up the puzzle over.
            if(puzzle != null)
            {
                puzzle.EndPuzzle();
                puzzleUI.puzzleWindowCover.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Calls LateStart.
            if (!calledLateStart)
                LateStart();

            // If the game is playing.
            if(stageManager.IsGamePlaying())
            {
                // Gets set to 'true' if the buttons are interactive.
                bool buttonsInter = stageManager.stageUI.IsAllActiveUnitButtonsInteractable();

                // This isn't efficent, but this is the best you can do without reworking the other systems greatly.

                // If the puzzle cover's active should be changed, change it.
                // If the buttons are interactable, the cover should be inactive, and vice-versa.
                if(puzzleUI.puzzleWindowCover.activeSelf == buttonsInter)
                {
                    puzzleUI.puzzleWindowCover.SetActive(!buttonsInter);
                }

            }
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