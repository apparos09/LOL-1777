using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_MST
{
    // The world manager.
    public class WorldManager : GameplayManager
    {
        // The stage count.
        public const int STAGE_COUNT = 9;

        // the instance of the class.
        private static WorldManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("WorldManager")]
        // The world user interface.
        public WorldUI worldUI;

        // The player in the world.
        public PlayerWorld playerWorld;

        // The world area.
        public WorldArea worldArea;

        // The game complete event.
        public GameCompleteEvent gameCompleteEvent;

        [Header("Area")]

        // The stage list.
        public List<StageWorld> stages = new List<StageWorld>();

        // The stage scene.
        public string stageScene = "StageScene";

        // Constructor
        private WorldManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
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
        protected override void Start()
        {
            base.Start();

            // TODO: check for save data first.

            // Sets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;

            // If the gameplay info has been instantiated.
            if (GameplayInfo.Instantiated)
            {
                // Load info into the world if it exists.
                if (gameInfo.hasWorldInfo)
                    gameInfo.LoadWorldInfo(this);
            }

            // If there are no stages in the stage list.
            if(stages.Count == 0)
            {
                // Finds all the stages in the world and puts them in the list.
                stages = new List<StageWorld>(FindObjectsOfType<StageWorld>(true));
            }

        }

        // The function called after the start function.
        protected override void LateStart()
        {
            base.LateStart();

            // The game complete event is set.
            if(gameCompleteEvent != null)
            {
                // Checks game complete to see if the game is finished.
                if(!gameCompleteEvent.cleared)
                    gameCompleteEvent.CheckGameComplete();
            }

            // Loads the test tutorial.
            // tutorials.LoadTutorialTest();
        }

        // Gets the instance.
        public static WorldManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<WorldManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("WorldManager (singleton)");
                        instance = go.AddComponent<WorldManager>();
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

        // Gets the stage index.
        public int GetStageWorldIndex(StageWorld stage)
        {
            return -1;
        }

        // SAVING/LOADING
        // Generates the save data or the game.
        public MST_GameData GenerateSaveData()
        {
            // The data.
            MST_GameData data = new MST_GameData();

            // TODO: implement.

            return data;
        }

        // Saves the data for the game.
        public bool SaveGame()
        {
            // If the LOL Manager does not exist, return false.
            if (!LOLManager.Instantiated)
            {
                Debug.LogError("The LOL Manager does not exist.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSys = LOLManager.Instance.saveSystem;

            // Checks if the save system exists.
            if (saveSys == null)
            {
                Debug.LogError("The save system could not be found.");
                return false;
            }


            // Set the world manager.
            if (saveSys.worldManager == null)
                saveSys.worldManager = this;

            // Saves the game.
            bool result = saveSys.SaveGame();

            // Return result.
            return result;
        }

        // Loads data, and return a 'bool' to show it was successful.
        public bool LoadGame()
        {
            // If the LOL Manager does not exist, return false.
            if (!LOLManager.Instantiated)
            {
                Debug.LogError("The LOL Manager does not exist.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSys = LOLManager.Instance.saveSystem;

            // Checks if the save system exists.
            if (saveSys == null)
            {
                Debug.LogError("The save system could not be found.");
                return false;
            }


            // Gets the loaded data.
            MST_GameData loadedData = saveSys.loadedData;

            // No data to load.
            if (loadedData == null)
            {
                Debug.LogError("The save data does not exist.");
                return false;
            }

            // Data invalid.
            if (loadedData.valid == false)
            {
                Debug.LogError("The save data is invalid.");
                return false;
            }

            // Game complete
            if (loadedData.complete)
            {
                // Changed from assertion to normal log.
                // Debug.LogAssertion("The game was completed, so the data hasn't been loaded.");
                Debug.Log("The game was completed, so the data hasn't been loaded.");
                return false;
            }

            // The data has been loaded successfully.
            return true;
        }


        // Goes to the stage.
        public void ToStage(StageWorld stageWorld)
        {
            // Saves the world info and goes into the stage.
            gameInfo.SaveWorldInfo(this);
            LoadStageScene();

            // TODO: play to stage animation?

        }

        // Loads the stage scene.
        public void LoadStageScene()
        {
            Time.timeScale = 1.0F;
            SceneManager.LoadScene(stageScene);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

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