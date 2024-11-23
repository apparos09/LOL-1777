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
        public List<PuzzlePiece> piecePrefabs = new List<PuzzlePiece>();

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            
        }

        // Start is called before the first frame update
        protected virtual void Start()
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

        // Initializes the puzzle for when a conversion question starts.
        public abstract void StartPuzzle();

        // Ends a puzzle when a meteor is untargeted.
        public abstract void EndPuzzle();

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}