using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // Moves the puzzle symbols along a path.
    public class PathPuzzle : Puzzle
    {
        // Awake is called when the script instance is being loaded.
        protected override void Awake()
        {
            base.Awake();
            puzzleType = PuzzleManager.puzzleType.path;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Initializes the puzzle for when a conversion question starts.
        public override void InitializePuzzle()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}