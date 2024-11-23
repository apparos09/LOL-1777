using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // Swaps the puzzle pieces around the screen.
    public class SwapPuzzle : Puzzle
    {
        [Header("SwapPuzzle")]

        // The list of symbol positions.
        public List<GameObject> symbolPositions;

        // Awake is called when the script instance is being loaded.
        protected override void Awake()
        {
            base.Awake();
            puzzleType = PuzzleManager.puzzleType.swap;   
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