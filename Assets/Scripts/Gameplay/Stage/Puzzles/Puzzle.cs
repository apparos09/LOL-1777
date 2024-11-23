using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The puzzle.
    public abstract class Puzzle : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The stage UI.
        public StageUI stageUI;

        // The puzzle manager.
        public PuzzleManager puzzleManager;

        // The puzzle UI.
        public PuzzleUI puzzleUI;

        // The type of the puzzle.
        protected PuzzleManager.puzzleType puzzleType = PuzzleManager.puzzleType.none;

        // The puzzle piece prefabs.
        public PuzzlePiece piecePrefab;

        // If set to 'true', initialize is called onStart for the puzzle.
        public bool initializeOnStart = true;

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Sets the instances needed for the puzzle to function.
            SetInstances();

            // Initializes the puzzle.
            if(initializeOnStart)
                InitializePuzzle();
        }

        // Sets the instances.
        public void SetInstances()
        {
            // Gets the stage manager.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // Gets the stage UI.
            if (stageUI == null)
                stageUI = StageUI.Instance;

            // Gets the puzzle manager.
            if (puzzleManager == null)
                puzzleManager = PuzzleManager.Instance;

            // Gets the puzzle UI.
            if (puzzleUI == null)
                puzzleUI = PuzzleUI.Instance;
        }

        // Returns this puzzle's type.
        public PuzzleManager.puzzleType GetPuzzleType()
        {
            return puzzleType;
        }

        // Initializes the puzzle.
        public abstract void InitializePuzzle();

        // Starts the puzzle.
        public abstract void StartPuzzle();

        // Stops the puzzle, which is called when a meteor is untargeted.
        public abstract void StopPuzzle();

        // Ends a puzzle when the game finishes.
        public abstract void EndPuzzle();

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // ...
        }
    }
}