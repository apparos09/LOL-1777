using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_MST
{
    // The tutorial script.
    public class Tutorials : MonoBehaviour
    {
        [System.Serializable]
        public struct TutorialData
        {
            // ...
        }

        // The tutorial types.
        public enum tutorialType
        {
            none
        };

        // The singleton instance.
        private static Tutorials instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game manager.
        public GameplayManager gameManager;

        // The tutorials UI.
        public TutorialUI tutorialsUI;

        // The tutorial type count.
        public const int TUTORIAL_TYPE_COUNT = 18;

        // The cleared tutorials.
        public List<tutorialType> clearedTutorials = new List<tutorialType>();

        // If 'true', the tutorials object constantly checks for starting tutorials.
        [Tooltip("Constant check for tutorial start.")]
        public bool constantTutorialStartCheck = true;

        // Constructor
        private Tutorials()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
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
            // Gets the game manager object.
            if (gameManager == null)
            {
                // Checks for the user interfaces to attach.
                if (WorldManager.Instantiated)
                {
                    gameManager = WorldManager.Instance;
                }
                else if (StageManager.Instantiated)
                {
                    gameManager = StageManager.Instance;
                }
                else
                {
                    Debug.LogWarning("Game manager could not be found.");
                }
            }

            // Gets the tutorials object.
            if (tutorialsUI == null)
                tutorialsUI = TutorialUI.Instance;
        }

        // Gets the instance.
        public static Tutorials Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<Tutorials>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Tutorial (singleton)");
                        instance = go.AddComponent<Tutorials>();
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

        // Checks if a tutorial is running.
        public bool IsTutorialRunning()
        {
            return tutorialsUI.IsTutorialRunning();
        }

        // Starts the tutorial.
        public void StartTutorial()
        {
            tutorialsUI.StartTutorial();
        }

        // Restarts the tutorial.
        public void RestartTutorial()
        {
            tutorialsUI.RestartTutorial();
        }

        // Ends the tutorial.
        public void EndTutorial()
        {
            tutorialsUI.EndTutorial();
        }

        // Called when a tutorial is started.
        public void OnTutorialStart()
        {
            // UI start function.
            tutorialsUI.OnTutorialStart();

            // Freeze the game.
            Time.timeScale = 0.0F;
        }

        // Called when a tutorail ends.
        public void OnTutorialEnd()
        {
            // UI end function.
            tutorialsUI.OnTutorialEnd();

            // Unfreeze the game if the game is not paused.
            if (!gameManager.IsGamePaused())
                Time.timeScale = 1.0F;

            // Ignore the current input for this frame in case the player is holding the space bar.
            // gameManager.player.IgnoreInputs(1);
        }



        // CLEARED TUTORIAL
        // Checks if the provided tutorial has been cleared.
        public bool IsTutorialCleared(tutorialType tutorial)
        {
            return clearedTutorials.Contains(tutorial);
        }

        // Adds a cleared tutorial to the list.
        public void AddClearedTutorial(tutorialType tutorial)
        {
            // If it's not in the list, add it
            if (!clearedTutorials.Contains(tutorial))
                clearedTutorials.Add(tutorial);
        }

        // Adds cleared tutorials to the list.
        public void AddClearedTutorials(List<tutorialType> trlList, bool clearList)
        {
            // Clears the tutorial list.
            if (clearList)
                clearedTutorials.Clear();

            // Adds all elements aside from duplicates.
            foreach (tutorialType trl in trlList)
            {
                AddClearedTutorial(trl);
            }
        }

        // Generates the cleared tutorials array.
        public bool[] GenerateClearedTutorialsArray()
        {
            // Creates the array.
            bool[] arr = new bool[TUTORIAL_TYPE_COUNT];

            // Fill the array.
            FillClearedTutorialsArray(ref arr);

            // Returns the array.
            return arr;
        }

        // Fills a bool array with cleared tutorials values. 
        public void FillClearedTutorialsArray(ref bool[] arr)
        {
            // Goes through the cleared tutorials list
            foreach (tutorialType trl in clearedTutorials)
            {
                // Convert the value.
                int index = (int)trl;

                // Index is valid.
                if (index >= 0 && index < arr.Length)
                {
                    // This value has been cleared.
                    arr[index] = true;
                }

            }
        }

        // Sets the cleared tutorials with the bool array.
        // If the array is a different length than the total amount of types...
        // The rest of the types are ignored.
        public void AddClearedTutorials(bool[] arr, bool clearList)
        {
            // Clears the tutorial list.
            if (clearList)
                clearedTutorials.Clear();

            // Goes through each index in the array.
            for (int i = 0; i < arr.Length && i < TUTORIAL_TYPE_COUNT; i++)
            {
                // Converts the index.
                tutorialType tutorial = (tutorialType)i;

                // If the tutorial has been cleared, add it to the list.
                if (arr[i] && !clearedTutorials.Contains(tutorial))
                {
                    // Adds the tutorial to the list.
                    clearedTutorials.Add(tutorial);
                }
            }
        }

        // Tutorial Loader

        // Loads the tutorial
        private void LoadTutorial(ref List<Page> pages)
        {
            // Loads pages for the tutorial.
            tutorialsUI.LoadPages(ref pages, false);
        }

        // Loads the tutorial of the provided type.
        public void LoadTutorial(tutorialType tutorial)
        {
            // ...
        }


        // Load the tutorial (template)
        private void LoadTutorialTemplate()
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new Page("Insert text here.")
            };

            // Change the display image when certain pages are opened using callbacks.

            // Loads the tutorial.
            LoadTutorial(ref pages);
        }


        // Update is called once per frame
        void Update()
        {
            // ...
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