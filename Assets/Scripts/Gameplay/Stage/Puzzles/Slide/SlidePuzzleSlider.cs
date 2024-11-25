using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace RM_MST
{
    // A slider line for the slide puzzle.
    public class SlidePuzzleSlider : MonoBehaviour
    {
        // The slide puzzle.
        public SlidePuzzle slidePuzzle;

        // The starting point.
        public GameObject startPoint;

        // The end point.
        public GameObject endPoint;

        // If 'true', pieces move in reverse across the line.
        public bool reversed = false;

        // The generated pieces.
        [HideInInspector]
        public List<PuzzlePiece> genPieces = new List<PuzzlePiece>();

        // The piece queue.
        public Queue<PuzzlePiece> piecePool = new Queue<PuzzlePiece>();

        // The index of the piece to be generated if the pool is empty (see base piece list).
        [Tooltip("The index of the base pieces list. This is used to know which next piece to generate.")]
        public int basePiecesIndex = 0;

        // The list of active pieces.
        public List<PuzzlePiece> activePieces = new List<PuzzlePiece>();

        // The piece spawn timer.
        public float pieceSpawnTimer = 0.0F;

        // Start is called before the first frame update
        void Start()
        {
            // If the slide puzzle isn't set, try to get the component in the parent.
            if(slidePuzzle == null)
            {
                slidePuzzle = GetComponentInParent<SlidePuzzle>();
            }
        }

        // Generates a puzzle piece.
        public PuzzlePiece GeneratePuzzlePiece()
        {
            // Clamps the index.
            basePiecesIndex = Mathf.Clamp(basePiecesIndex, 0, slidePuzzle.basePieces.Count - 1);

            // Instantiates the new piece and makes the object active.
            PuzzlePiece newPiece = Instantiate(slidePuzzle.basePieces[basePiecesIndex]);
            newPiece.gameObject.SetActive(true);

            // Make this piece have this slider as a parent, and reset the local position.
            newPiece.transform.parent = transform;
            newPiece.transform.localPosition = Vector3.zero;

            // Set piece at start or end point based on the reversed parameter.
            newPiece.transform.position = (reversed) ? endPoint.transform.position : startPoint.transform.position;

            // Increase the index.
            basePiecesIndex++;

            // Out of bounds, so loop around to the beginning.
            if(basePiecesIndex >= slidePuzzle.basePieces.Count)
            {
                basePiecesIndex = 0;
            }

            // Returns the new piece.
            return newPiece;

        }

        // Spawns a puzzle piece.
        public PuzzlePiece SpawnPuzzlePiece()
        {
            // The piece to be gotten.
            PuzzlePiece piece;

            // The piece pool.
            if(piecePool.Count > 0)
            {
                // Grab a piece from the pool.
                piece = piecePool.Dequeue();
            }
            else
            {
                // Generate a new piece.
                piece = GeneratePuzzlePiece();
            }

            // Activate the piece.
            piece.gameObject.SetActive(true);

            // Check that parent is set correctly.
            if(piece.transform.parent != transform)
                piece.transform.parent = transform;

            // Gets the spawn point, and uses it to set the piece's position.
            GameObject spawnPoint = (reversed) ? endPoint : startPoint;
            piece.gameObject.transform.position = spawnPoint.transform.position;

            // Add the piece to the active list.
            activePieces.Add(piece);

            // Return the piece.
            return piece;
        }

        // Returns the puzzle piece to the pool.
        public void ReturnPuzzlePieceToPool(PuzzlePiece piece)
        {
            // Removes the piece from the active list if it's in there.
            if(activePieces.Contains(piece))
            {
                activePieces.Remove(piece);
            }

            // Turn the piece off.
            piece.gameObject.SetActive(false);

            // Put the piece in the pool.
            piecePool.Enqueue(piece);
        }

        // Destroys all the puzzle pieces.
        public void DestroyAllPuzzlePieces()
        {
            // Destroys all the active pieces.
            foreach(PuzzlePiece piece in activePieces)
            {
                // Piece exists, so destroy it.
                if(piece != null)
                {
                    Destroy(piece.gameObject);
                }
            }

            // While the piece pool has pieces, destroy them.
            while(piecePool.Count > 0)
            {
                // Pull the piece from the list.
                PuzzlePiece piece = piecePool.Peek();
                piecePool.Dequeue();

                // Destroy the piece.
                if(piece != null)
                {
                    Destroy(piece.gameObject);
                }
            }

            // Clear the active pieces and piece pool.
            activePieces.Clear();
            piecePool.Clear();
        }

        // Sets the spawn timer to the max.
        public void ResetPieceSpawnTimer()
        {
            pieceSpawnTimer = slidePuzzle.pieceSpawnTimeMax;
        }

        // Runs the slider line.
        public void RunSlider()
        {
            // Gets the destination point.
            // If going in the normal direct, go from start to end.
            // If going in the reverse direction, go from end to start.
            GameObject destPoint = (reversed) ? startPoint : endPoint;

            // Calculates the movement speed.s
            float moveSpeed = slidePuzzle.speed * Time.deltaTime;

            // Goes from end to start, updating the active pieces.
            for(int i = activePieces.Count - 1; i >= 0; i--)
            {
                // Active piece exists.
                if (activePieces[i] != null)
                {
                    // Gets the piece.
                    PuzzlePiece piece = activePieces[i];

                    // Calculates the new position.
                    Vector3 newPos = Vector3.MoveTowards(piece.transform.position, destPoint.transform.position, moveSpeed);

                    // Sets the new position.
                    piece.transform.position = newPos;

                    // The active piece is at the destination point, so return it to the list.
                    if (newPos == destPoint.transform.position)
                    {
                        // Remove the piece from the active list.
                        activePieces.RemoveAt(i);

                        // Returns the puzzle piece to the pool.
                        // This function also removes it from the active list if it's there.
                        ReturnPuzzlePieceToPool(piece);
                    }
                }
                else // Active piece doesn't exist, so remove it from the list.
                {
                    activePieces.RemoveAt(i);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // The slide puzzle is set.
            if(slidePuzzle != null)
            {
                // The sliders are running.
                if(slidePuzzle.GetSlidersRunning())
                {
                    // Run the slider.
                    RunSlider();

                    // Reduce the timer.
                    pieceSpawnTimer -= Time.unscaledDeltaTime;

                    // Generate a piece.
                    if(pieceSpawnTimer <= 0.0F)
                    {
                        // Set to 0.
                        pieceSpawnTimer = 0.0F;

                        // Spawn a puzzle piece.
                        SpawnPuzzlePiece();

                        // Reset the timer.
                        ResetPieceSpawnTimer();
                    }
                }
            }
        }

        // This function is called when the MonoBehaviour will be destroyed
        private void OnDestroy()
        {
            DestroyAllPuzzlePieces();
        }
    }
}