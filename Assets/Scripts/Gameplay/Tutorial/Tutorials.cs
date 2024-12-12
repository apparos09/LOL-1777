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

            public bool clearedHiddenMultiplesTutorial;
            public bool clearedBarriersTutorial;

            public bool clearedPuzzleButtonsTutorial;
            public bool clearedPuzzleSwapTutorial;
            public bool clearedPuzzleSlideTutorial;
            public bool clearedPuzzlePathTutorial;

            public bool clearedFirstWinTutorial;
            public bool clearedMixStageTutorial;

            public bool clearedLengthImperial;
            public bool clearedWeightImperial;
            public bool clearedTimeTutorial;

            public bool clearedLengthMetricTutorial;
            public bool clearedWeightMetricTutorial;
            public bool clearedCapacityTutorial;
        }

        // The tutorial types 
        // TODO: remove.
        // public enum tutorialType
        // {
        //     none, intro, stage, firstWin, mixStage, weightImperial, lengthImperial, time, lengthMetric, weightMetric, capacity
        // };


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

        public bool clearedHiddenMultiplesTutorial;
        public bool clearedBarrierTutorial;

        public bool clearedPuzzleButtonsTutorial;
        public bool clearedPuzzleSwapTutorial;
        public bool clearedPuzzleSlideTutorial;
        public bool clearedPuzzlePathTutorial;

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

        // Called when a tutorial ends.
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

            data.clearedHiddenMultiplesTutorial = clearedHiddenMultiplesTutorial;
            data.clearedBarriersTutorial = clearedBarrierTutorial;

            data.clearedPuzzleButtonsTutorial = clearedPuzzleButtonsTutorial;
            data.clearedPuzzleSwapTutorial = clearedPuzzleSwapTutorial;
            data.clearedPuzzleSlideTutorial = clearedPuzzleSlideTutorial;
            data.clearedPuzzlePathTutorial = clearedPuzzlePathTutorial;

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

            clearedHiddenMultiplesTutorial = data.clearedHiddenMultiplesTutorial;
            clearedBarrierTutorial = data.clearedBarriersTutorial;

            clearedPuzzleButtonsTutorial = data.clearedPuzzleButtonsTutorial;
            clearedPuzzleSwapTutorial = data.clearedPuzzleSwapTutorial;
            clearedPuzzleSlideTutorial = data.clearedPuzzleSlideTutorial;
            clearedPuzzlePathTutorial = data.clearedPuzzlePathTutorial;

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

        // // Loads the tutorial of the provided type.
        // public void LoadTutorial(tutorialType tutorial)
        // {
        //     // ...
        // }


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
                new MST_Page("Welcome to the Meteor Strike Team (MST)! Our job is to track down and destroy meteors before they hit the Earth's surface, which we do by converting larger measurement units into smaller measurement units. I'm Reteor...", "trl_intro_00"),
                new MST_Page("And I'm Astrite! This is the world area, which is where you select stages, view unit information, change the game settings, and save your game. When you select a stage, the relevant measurement information is shown. Once you get said information, it can be viewed again by selecting the 'units info' button.", "trl_intro_01"),
                new MST_Page("With all that explained, please select the available stage to start destroying meteors!", "trl_intro_02"),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[1].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[2].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);

            // If the world manager UI is instantiated.
            if(WorldUI.Instantiated)
            {
                // Disable the units info button since there's nothing to display yet.
                WorldUI worldUI = WorldUI.Instance;
                pages[0].OnPageOpenedAddCallback(worldUI.MakeUnitsInfoButtonUninteractable);
            }

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
                new MST_Page("Welcome to the stage area, which is where you'll shoot down meteors. The meteor closest to the Earth's surface is automatically targeted, so all you must do is solve the conversions presented to you. You get points for performing successful conversions, and once you get enough points, the stage is completed. But if the Earth's surface takes too much damage, the stage is lost.", "trl_firstStage_00"),
                new MST_Page("When a meteor is targeted, you are presented with different conversion outputs and the multipliers that were used to get them. Once you select an output, a laser shot is fired at the targeted meteor. The method for choosing an output will vary by stage.", "trl_firstStage_01"),
                new MST_Page("If you've selected the correct output, the targeted meteor is destroyed, and the rest of the meteors are pushed back. If you've selected an incorrect output, only the targeted meteor is knocked back, and said meteor is not destroyed. You will also be briefly stunned if you choose a wrong output.", "trl_firstStage_02"),
                new MST_Page("On the left are the time, the points, the points bar, and the surface's health bar. On the right are the settings button, the world button, the speed button, and the unit conversion table. Feel free to change the game's speed using the speed button if you want more time to do unit conversions. With all that said, time to start the stage!", "trl_firstStage_03"),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[1].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[2].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[3].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);


            // Sets the bool and loads the tutorial.
            clearedFirstStageTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the hidden mutliples tutorial.
        public void LoadHiddenMultiplesTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("The multipliers have become hidden! If you perform enough conversions correctly in a row, the conversion multipliers will disappear. They will come back a few seconds after a conversion question is given, but this will give more time for meteors to reach the Earth's surface. If you get a conversion wrong, the multipliers will start showing up instantly like before, unless you get enough consecutive correct answers again.", "trl_hiddenMultiples_00")
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedHiddenMultiplesTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the barriers tutorial.
        public void LoadBarrierTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("A barrier has been hit! When a meteor hits a barrier, the meteor is destroyed, but the barrier takes damage. If a barrier takes too much damage, it'll be destroyed, leaving an opening for meteors to hit the Earth's surface and damage it. Once you get enough points, a barrier is restored.", "trl_barrier_00")
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedBarrierTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }


        // Puzzles
        // Loads the puzzle - unit buttons tutorial.
        public void LoadPuzzleUnitsButtonsTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("This is a units buttons puzzle! Submit an output by selecting the button its attached to.", "trl_pzl_buttons")
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedPuzzleButtonsTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the puzzle - swap tutorial.
        public void LoadPuzzleSwapTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("This is a swap puzzle! Symbols switch places when the time bar in the puzzle area fully depletes. Select an output using one of its corresponding symbols in the puzzle area.", "trl_pzl_swap")
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedPuzzleSwapTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the puzzle - slide tutorial.
        public void LoadPuzzleSlideTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("This is a slide puzzle! Symbols slide across the puzzle area while a meteor is targeted. Select an output using one of its corresponding symbols in the puzzle area.", "trl_pzl_slide")
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedPuzzleSlideTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the puzzle - path tutorial.
        public void LoadPuzzlePathTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("This is a path puzzle! Symbols move along the path in the puzzle area while a meteor is targeted. Select an output using its corresponding symbol in the puzzle area.", "trl_pzl_path")
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedPuzzlePathTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }


        // First Win and Mix
        // Loads the first win tutorial.
        public void LoadFirstWinTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("You've completed the first stage, which has unlocked even more stages! The more stages you beat, the more stages you'll unlock. Of the stages available, you can clear them in any order, but you must beat all the stages to complete the game.", "trl_firstWin_00"),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

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
                new MST_Page("This is a mix stage. Mix stages have you deal with multiple unit groups at once. You'll only deal with a mix stage after you've experienced all relevant unit groups.", "trl_mixStage_00"),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);


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
                new MST_Page("These units are used to measure how long something is. 1 yard (yd) is equal to 3 feet (ft), and 1 foot (ft) is equal to 12 inches (in).", "trl_lengthImperial_00"),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerA);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);

            // Sets the bool and loads the tutorial.
            clearedLengthImperialTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the weight (imperial) tutorial.
        public void LoadWeightImperialTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new MST_Page("These units are used to measure how heavy someone or something is. 1 pound (lb) is equal to 16 ounces (oz).", "trl_weightImperial_00"),
            };

            // Change the display image when certain pages are opened using callbacks.
            pages[0].OnPageOpenedAddCallback(tutorialsUI.SetCharacterToPartnerB);
            pages[0].OnPageOpenedAddCallback(tutorialsUI.textBox.ShowCharacterImage);


            // If the world manager UI is instantiated.
            if (WorldUI.Instantiated)
            {
                // This is the first stage of the game.
                // As such, it makes sure to turn on the units button now that this tutorial has been given.
                WorldUI worldUI = WorldUI.Instance;
                pages[0].OnPageClosedAddCallback(worldUI.MakeUnitsInfoButtonInteractable);
            }


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
                new MST_Page("These units are used to measure lengths of time. 1 hour (hrs) is equal to 60 minutes (mins), and 1 minute (mins) is equal to 60 seconds (secs).", "trl_time_00"),
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
                new MST_Page("These units are used to measure how long something is. 1 kilometer (km) is equal to 1000 meters (m), 1 meter is equal to 100 centimeters (cm) and 1000 millimeters (mm), 1 decimeter (dm) is equal to 10 centimeters (cm), and 1 centimeter (cm) is equal to 10 millimeters (mm).", "trl_lengthMetric_00"),
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
                new MST_Page("These units are used to measure how heavy someone or something is. 1 kilogram (kg) is equal to 1000 grams (g), and 1 gram (g) is equal to 1000 milligrams (mg).", "trl_weightMetric_00"),
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
                new MST_Page("These units are used to measure the volume of a container. 1 liter (l) is equal to 1000 millilitres (mL).", "trl_capacity_00"),
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