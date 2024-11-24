using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_MST
{
    // Swaps the puzzle pieces around the screen.
    public class SwapPuzzle : Puzzle
    {
        [Header("SwapPuzzle")]

        // The list of symbol positions.
        public List<GameObject> piecePositions;

        // The generated pieces.
        protected List<PuzzlePiece> genPieces = new List<PuzzlePiece>();

        // If 'true', pieces are duplicated to remaining spaces.
        private bool duplicatePieces = true;

        // Determines if symbols get swapped.
        // Becomes 'true' when the puzzle starts.
        protected bool swappingEnabled = false;

        // The timer used for swapping piece positions.
        private float swapTimer = 0.0F;

        // The maximum time it takes to swap piece positions.
        public const float SWAP_TIMER_MAX = 4.0F;

        // The list of positions used for swapping.
        private List<GameObject> swapPosList = new List<GameObject>();

        // The progress bar that represents the swap timer.
        public ProgressBar swapTimerBar;

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
            List<PuzzleConversionDisplay> displays = puzzleManager.puzzleUI.GenerateActiveConversionDisplayList();

            // There are no unit buttons.
            if (unitsButtons.Count < 0 || displays.Count < 0)
            {
                Debug.LogError("No active unit buttons were found! Puzzle will fail to load.");
            }

            // The display index.
            int displayIndex = 0;

            // The symbol index.
            int symbolIndex = -1;

            // The queue of instantiated symbols.
            Queue<PuzzlePiece>instSymbols = new Queue<PuzzlePiece>();

            // While there are positions to be filled.
            // TODO: loop around if the display index passes the list count.
            for (int i = 0; i < piecePositions.Count && displayIndex < displays.Count; i++)
            {
                // Instantiates the piece.
                PuzzlePiece piece = Instantiate(piecePrefab);

                // Set the conversion display.
                piece.SetPieceFromConversionDisplay(displays[displayIndex]);

                // Disable since it's not needed.
                piece.setDisplayInfoOnStart = false;

                // Add the piece to the symbol position via parenting.
                piece.transform.parent = piecePositions[i].transform;
                piece.transform.localPosition = Vector3.zero;

                // Add the piece to the gen list.
                genPieces.Add(piece);

                // Add a piece to the instantiated list.
                instSymbols.Enqueue(piece);

                // Increase the display index.
                displayIndex++;

                // Saves the symbol index.
                symbolIndex = i;
            }

            // Increases by 1 since the loop is over.
            symbolIndex++;

            // If there should be duplicate pieces.
            if(duplicatePieces)
            {
                // While there are displays left to fill.
                while (symbolIndex < piecePositions.Count)
                {
                    // Gets the original piece, removes it from the queue.
                    PuzzlePiece origPiece = instSymbols.Peek();
                    instSymbols.Dequeue();

                    // Copy the original piece and put it back in the queue.
                    PuzzlePiece copyPiece = Instantiate(origPiece);
                    instSymbols.Enqueue(origPiece);

                    // Change the parent of the copy piece, and set its local position to 0.
                    copyPiece.transform.parent = piecePositions[symbolIndex].transform;
                    copyPiece.transform.localPosition = Vector3.zero;

                    // Add the copied piece to the generated pieces list.
                    genPieces.Add(copyPiece);

                    // Increase the symbol index.
                    symbolIndex++;
                }
            }
            

            // Makes the swap position list and resets the timer to start off.
            swapPosList.Clear();
            swapPosList = new List<GameObject>(piecePositions);
            ResetPuzzleSwapTimer();
        }

        // Initializes the puzzle for when a conversion question starts.
        public override void StartPuzzle()
        {
            // Resets the swap timer.
            ResetPuzzleSwapTimer();

            // Swapping is enabled.
            swappingEnabled = true;
        }

        // Stops the puzzle, which is called when a meteor is untargeted.
        public override void StopPuzzle()
        {
            // Resets the piece positions.
            // ResetPiecePositions();

            // Reset the timer.
            ResetPuzzleSwapTimer();

            // Swaping is disabled.
            swappingEnabled = false;
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

        // Swapping
        // Checks if swapping is enabled.
        public bool IsSwappingEnabled()
        {
            return swappingEnabled;
        }


        // Swaps the symbol positions.
        public void SwapPiecePositions()
        {
            // Makes a queue out of the piece list and the positions list.
            // Pieces
            Queue<PuzzlePiece> pieceQueue = new Queue<PuzzlePiece>(genPieces);
            
            // Positions
            Queue<GameObject> posQueue;

            // If the lists are not same length, reset the swap position list.
            if (swapPosList.Count != genPieces.Count)
            {
                swapPosList.Clear();
                swapPosList = new List<GameObject>(piecePositions);
            }

            // Make a queue out of the swap position list.
            posQueue = new Queue<GameObject>(swapPosList);

            // There are no pieces, or no positions.
            if (pieceQueue.Count <= 0 || posQueue.Count <= 0)
            {
                Debug.LogWarning("There are either no pieces or no positions.");
                return;
            }

            // Moves the first position to the end of the list so...
            // That positions can be swapped.
            GameObject tempObject = posQueue.Peek();
            posQueue.Dequeue();
            posQueue.Enqueue(tempObject);

            // The list of new positions.
            List<GameObject> newPosList = new List<GameObject>();

            // While there are pieces and positions.
            while(pieceQueue.Count > 0 && posQueue.Count > 0)
            {
                // Grabs the new position.
                GameObject newPos = posQueue.Peek();
                posQueue.Dequeue();

                // Adds the position to the new pos list.
                newPosList.Add(newPos);

                // Grabs the piece, change it's parent, and reset it's local position.
                PuzzlePiece piece = pieceQueue.Dequeue();
                piece.transform.parent = newPos.transform;
                piece.transform.localPosition = Vector3.zero;
            }

            // Replace the old swap pos list with the new pos list.
            swapPosList.Clear();
            swapPosList = new List<GameObject>(newPosList);
        }

        // Resets the piece positions.
        public void ResetPiecePositions()
        {
            // Makes a queue out of the generated pieces and the piece positions.
            Queue<PuzzlePiece> pieceQueue = new Queue<PuzzlePiece>(genPieces);
            Queue<GameObject> posQueue = new Queue<GameObject>(piecePositions);

            // Clear the sawp position list, and make a new one with the piece position order.
            swapPosList.Clear();
            swapPosList = new List<GameObject>(piecePositions);

            // While there are pieces and positions.
            while (pieceQueue.Count > 0 && posQueue.Count > 0)
            {
                // Grabs the new position.
                GameObject newPos = posQueue.Peek();
                posQueue.Dequeue();

                // Grabs the piece, change it's parent, and reset it's local position.
                PuzzlePiece piece = pieceQueue.Dequeue();
                piece.transform.parent = newPos.transform;
                piece.transform.localPosition = Vector3.zero;
            }
        }

        // Resets the puzzle swap timer.
        public void ResetPuzzleSwapTimer()
        {
            swapTimer = SWAP_TIMER_MAX;
        }

        // Updates the swap timer progress bar.
        public void UpdateSwapTimerProgressBar()
        {
            // The percent.
            float percent;

            // Gets the swap max value.
            float swapTimerMax = SWAP_TIMER_MAX;

            // Checks that the swap timer max is not 0.
            if(swapTimerMax > 0)
            {
                percent = swapTimer / swapTimerMax;
            }
            else // max is 0.
            {
                percent = 0.0F;
            }

            // Clamp into [0.0, 1.0] bounds.
            percent = Mathf.Clamp01(percent);

            // Sets the value.
            swapTimerBar.SetValueAsPercentage(percent);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If swapping is enabled and the game is being played.
            if(swappingEnabled && stageManager.IsGamePlaying())
            {
                // Reduce the timer.
                swapTimer -= Time.unscaledDeltaTime;
                
                // Bounds check.
                if(swapTimer < 0.0F)
                {
                    swapTimer = 0.0F;
                }

                // Time to swap positions.
                if(swapTimer <= 0.0F)
                {
                    // Swap the positions and reset the timer.
                    SwapPiecePositions();
                    ResetPuzzleSwapTimer();
                }

                // Updates the progress bar.
                UpdateSwapTimerProgressBar();
            }
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