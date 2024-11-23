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

        // The generated pieces.
        protected List<PuzzlePiece> genPieces = new List<PuzzlePiece>();

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

        // Initializes the puzzle.
        public override void InitializePuzzle()
        {
            // Generate a list of the unit buttons.
            List<UnitsButton> unitsButtons = stageUI.GenerateUnitsButtonsActiveList();
            List<PuzzleConversionDisplay> displays = puzzleManager.puzzleUI.GenerateConversionDisplayList();

            // There are no unit buttons.
            if (unitsButtons.Count < 0 || displays.Count < 0)
            {
                Debug.LogError("No active unit buttons were found! Puzzle will fail to load.");
            }

            // The display index.
            int displayIndex = 0;

            // While there are positions to be filled.
            // TODO: loop around if the display index passes the list count.
            for (int i = 0; i < symbolPositions.Count && displayIndex < displays.Count; i++)
            {
                // Instantiates the piece.
                PuzzlePiece piece = Instantiate(piecePrefab);

                // Set the conversion display.
                piece.SetPieceFromConversionDisplay(displays[displayIndex]);

                // Disable since it's not needed.
                piece.setDisplayInfoOnStart = false;

                // Add the piece to the symbol position via parenting.
                piece.transform.parent = symbolPositions[i].transform;
                piece.transform.localPosition = Vector3.zero;

                // Add the piece to the list.
                genPieces.Add(piece);

                // Increase the display index.
                displayIndex++;
            }
        }

        // Initializes the puzzle for when a conversion question starts.
        public override void StartPuzzle()
        {
            // ...
        }

        // Stops the puzzle, which is called when a meteor is untargeted.
        public override void StopPuzzle()
        {
            // ...
        }

        // Ends a puzzle when a meteor is untargeted.
        public override void EndPuzzle()
        {
            // Destroys all the generated pieces.
            foreach (PuzzlePiece piece in genPieces)
            {
                // Destroys the generated piece.
                if (piece != null)
                {
                    Destroy(piece.gameObject);
                }
            }

            // Clears out the list.
            genPieces.Clear();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Ends the puzzle.
            // TODO: this may have already been called.
            EndPuzzle();
        }
    }
}