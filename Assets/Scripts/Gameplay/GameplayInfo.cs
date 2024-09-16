using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // This script is used for game info that's shared between all scenes.
    public class GameplayInfo : MonoBehaviour
    {
        // The information for starting a stage.
        public struct StageStartInfo
        {
            // Valid - see if the stage start info is valid.
            public bool valid;

            // Name
            public string name;

            // Unit Groups
            public List<UnitsInfo.unitGroups> stageUnitGroups;

            // Difficulty
            public int difficulty;

            // Index in the Stage List
            public int index;
        }

        // The singleton instance.
        private static GameplayInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game time.
        public float gameTime = 0.0F;

        // The game score.
        public int gameScore = 0;

        [Header("WorldInfo")]

        // If the class has world info.
        public bool hasWorldInfo = false;

        // Marks if the stage has been cleared or not.
        public bool[] clearedStages = new bool[WorldManager.STAGE_COUNT];

        // The stage start information.
        public StageStartInfo stageStartInfo;

        [Header("StageInfo")]

        // If the class has stage info.
        public bool hasStageInfo = false;
        
        // Constructor
        private GameplayInfo()
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
        private void Start()
        {
            // Don't destroy this game object on load.
            DontDestroyOnLoad(gameObject);

            // Blank data to start.
            stageStartInfo = new StageStartInfo();
            stageStartInfo.valid = false; // Don't read from this.
        }

        // Gets the instance.
        public static GameplayInfo Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<GameplayInfo>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Game Info (singleton)");
                        instance = go.AddComponent<GameplayInfo>();
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

        // Saves info from the game manager.
        public void SaveGameplayInfo(GameplayManager gameManager)
        {
            gameTime = gameManager.gameTime;
            gameScore = gameManager.gameScore;
        }

        // Loads game info into the game manager.
        public void LoadGameplayInfo(GameplayManager gameManager)
        {
            gameManager.gameTime = gameTime;
            gameManager.gameScore = gameScore;
        }


        // Saves the world info from the world manager object.
        public void SaveWorldInfo(WorldManager worldManager)
        {
            SaveGameplayInfo(worldManager);

            // Tries to grab the next stage.
            StageWorld nextStage = worldManager.worldUI.stageWorldUI.stageWorld;

            // Next stage is set.
            if (nextStage != null)
            {
                // Generates the stage info and sets it.
                stageStartInfo = nextStage.GenerateStageInfo();
            }

            // Saves what stages have been cleared.
            for(int i = 0; i < clearedStages.Length && i < worldManager.stages.Count; i++)
            {
                // If there is a stage, get the clear value.
                if (worldManager.stages[i] != null)
                    clearedStages[i] = worldManager.stages[i].cleared;
            }

            // There is world info.
            hasWorldInfo = true;
        }

        // Loads the world info into the world object.
        public void LoadWorldInfo(WorldManager worldManager)
        {
            LoadGameplayInfo(worldManager);

            // Sets what stages have been cleared.
            for (int i = 0; i < clearedStages.Length && i < worldManager.stages.Count; i++)
            {
                // If there is a stage, set the clear value.
                if (worldManager.stages[i] != null)
                    worldManager.stages[i].cleared = clearedStages[i];
            }
        }

        // Saves the stage info from the stage manager object.
        public void SaveStageInfo(StageManager stageManager)
        {
            SaveGameplayInfo(stageManager);

            // There is stage info.
            hasStageInfo = true;
        }

        // Loads the stage info into the stage object.
        public void LoadStageInfo(StageManager stageManager)
        {
            LoadGameplayInfo(stageManager);

            // Apply the stage start info.
            stageManager.ApplyStageStartInfo(stageStartInfo);
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