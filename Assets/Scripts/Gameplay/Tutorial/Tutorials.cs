using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_MST
{
    // The tutorial script.
    public class Tutorials : MonoBehaviour
    {
        [System.Serializable]
        public class TutorialsData
        {
            public bool clearedIntroTutorial;
            public bool clearedFirstStageTutorial;
            public bool clearedFirstWinTutorial;
            public bool clearedMixStageTutorial;

            public bool clearedLengthImperial;
            public bool clearedWeightImperial;
            public bool clearedTimeTutorial;

            public bool clearedLengthMetricTutorial;
            public bool clearedWeightMetricTutorial;
            public bool clearedCapacityTutorial;
        }

        // The tutorial types.
        // TODO: add the rest.
        public enum tutorialType
        {
            none, intro, stage, firstWin, mixStage, weightImperial, lengthImperial, time, lengthMetric, weightMetric, capacity
        };


        // The tutorial type count.
        public const int TUTORIAL_TYPE_COUNT = 7;

        // The singleton instance.
        private static Tutorials instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game manager.
        public GameplayManager gameManager;

        // The tutorials UI.
        public TutorialUI tutorialsUI;

        // // If 'true', the tutorials object constantly checks for starting tutorials.
        // [Tooltip("Constant check for tutorial start.")]
        // public bool constantTutorialStartCheck = true;


        [Header("Tutorials")]

        public bool clearedIntroTutorial;
        public bool clearedFirstStageTutorial;
        public bool clearedFirstWinTutorial;
        public bool clearedMixStageTutorial;

        public bool clearedLengthImperialTutorial;
        public bool clearedWeightImperialTutorial;
        public bool clearedTimeTutorial;

        public bool clearedLengthMetricTutorial;
        public bool clearedWeightMetricTutorial;
        public bool clearedCapacityTutorial;

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
                    // Tries to find the object.
                    gameManager = FindObjectOfType<GameplayManager>();

                    // Not set, so state a warning.
                    if(gameManager == null)
                        Debug.LogWarning("Game manager could not be found.");
                }
            }

            // Gets the tutorials object.
            if (tutorialsUI == null)
                tutorialsUI = TutorialUI.Instance;

            // Don't destroy this game object.
            DontDestroyOnLoad(gameObject);
        }


        // This function is called when the object is enabled and active
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
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

        // Called when the scene is loaded.
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // If the game manager is not set, set it.
            if (gameManager == null)
            {
                // Try to find the game manager.
                gameManager = FindObjectOfType<GameplayManager>();
            }

            // Try to get the tutorials UI again.
            if(tutorialsUI == null)
                tutorialsUI = TutorialUI.Instance;
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
            {
                // If the game manager is set, check it for the time scale.
                // If it's not set, use 1.0F.
                if(gameManager != null)
                {
                    Time.timeScale = gameManager.GetGameTimeScale();
                }
                else
                {
                    Time.timeScale = 1.0F;
                }
                
            }
                
            // Ignore the current input for this frame in case the player is holding the space bar.
            // gameManager.player.IgnoreInputs(1);
        }
        

        // TUTORIAL DATA
        // Generates the tutorials data.
        public TutorialsData GenerateTutorialsData()
        {
            TutorialsData data = new TutorialsData();
            
            data.clearedIntroTutorial = clearedIntroTutorial;
            data.clearedFirstStageTutorial = clearedFirstStageTutorial;

            data.clearedFirstWinTutorial = clearedFirstWinTutorial;
            data.clearedMixStageTutorial = clearedMixStageTutorial;

            data.clearedWeightImperial = clearedWeightImperialTutorial;
            data.clearedLengthImperial = clearedLengthImperialTutorial;
            data.clearedTimeTutorial = clearedTimeTutorial;

            data.clearedLengthMetricTutorial = clearedLengthMetricTutorial;
            data.clearedWeightMetricTutorial = clearedWeightMetricTutorial;
            data.clearedCapacityTutorial = clearedCapacityTutorial;

            return data;
        }

        // Sets the tutorials data.
        public void LoadTutorialsData(TutorialsData data)
        {
            clearedIntroTutorial = data.clearedIntroTutorial;
            clearedFirstStageTutorial = data.clearedFirstStageTutorial;
            clearedFirstWinTutorial = data.clearedFirstWinTutorial;
            clearedMixStageTutorial = data.clearedMixStageTutorial;

            clearedWeightImperialTutorial = data.clearedWeightImperial;
            clearedLengthImperialTutorial = data.clearedLengthImperial;
            clearedTimeTutorial = data.clearedTimeTutorial;

            clearedLengthMetricTutorial = data.clearedLengthMetricTutorial;
            clearedWeightMetricTutorial = data.clearedWeightMetricTutorial;
            clearedCapacityTutorial = data.clearedCapacityTutorial;
        }


        // Tutorial Loader

        // Loads the tutorial
        private void LoadTutorial(ref List<Page> pages, bool startTutorial = true)
        {
            // The gameplay manager isn't set, try to find it.
            if (gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>();

            // Loads pages for the tutorial.
            if(gameManager != null && startTutorial) // If the game manager is set, start the tutorial.
            {
                gameManager.StartTutorial(pages);
            }
            else // No game manager, so just load the pages.
            {
                tutorialsUI.LoadPages(ref pages, false);
            }
        }

        // Loads the tutorial of the provided type.
        public void LoadTutorial(tutorialType tutorial)
        {
            // ...
        }


        // Load the tutorial (template)
        private void LoadTutorialTemplate(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new Page("Insert text here.")
            };

            // Change the display image when certain pages are opened using callbacks.

            // Loads the tutorial.
            LoadTutorial(ref pages, startTutorial);
        }

        // Load test tutorial
        public void LoadTutorialTest(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("This is a test."),
                new MST_Page("This is only a test.")
            };

            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);
            pages[1].OnPageOpenedAddCallback(tutorialsUI.textBox.HideCharacterImage);


            // Change the display image when certain pages are opened using callbacks.

            // Loads the tutorial.
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the intro tutorial.
        public void LoadIntroTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.

            // Sets the bool and loads the tutorial.
            clearedIntroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the stage tutorial.
        public void LoadFirstStageTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.

            // Sets the bool and loads the tutorial.
            clearedFirstStageTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first win tutorial.
        public void LoadFirstWinTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.

            // Sets the bool and loads the tutorial.
            clearedFirstWinTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the mix stage tutorial.
        public void LoadMixStageTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.

            // Sets the bool and loads the tutorial.
            clearedMixStageTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Measurement Groups
        // Loads the length (imperial) tutorial.
        public void LoadLengthImperialTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedMixStageTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the weight (imperial) tutorial.
        public void LoadWeightImperialTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedWeightImperialTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the time tutorial.
        public void LoadTimeTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedTimeTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the length metric tutorial.
        public void LoadLengthMetricTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedLengthMetricTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the weight metric tutorial.
        public void LoadWeightMetricTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedWeightMetricTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the weight metric tutorial.
        public void LoadCapacityTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("..."),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedCapacityTutorial = true;
            LoadTutorial(ref pages, startTutorial);
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