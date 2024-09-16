using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_MST
{
    // The stage manager.
    public class StageManager : GameplayManager
    {
        // the instance of the class.
        private static StageManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Tooltip("StageManager")]
        // The stage user interface.
        public StageUI stageUI;

        // The stage.
        public Stage stage;

        // The stage's name.
        public string stageName;

        // The difficulty.
        public int difficulty = 0;

        // The stage index.
        public int stageIndex = -1;

        // The units used for the stage.
        public List<UnitsInfo.unitGroups> stageUnitGroups = new List<UnitsInfo.unitGroups>();

        // The world scene.
        public string worldScene = "WorldScene";

        // Constructor
        private StageManager()
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

            // If the gameplay info has been instantiated.
            if (GameplayInfo.Instantiated)
            {
                // Load the stage information.
                gameInfo.LoadStageInfo(this);
            }
            else
            {
                // If there are no stage units, generate the group list.
                if(stageUnitGroups.Count == 0)
                    stageUnitGroups = UnitsInfo.GenerateUnitGroupsList();
            }


        }

        // The function called after the start function.
        protected override void LateStart()
        {
            base.LateStart();

            // TODO: start stage.
        }

        // Gets the instance.
        public static StageManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<StageManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("WorldManager (singleton)");
                        instance = go.AddComponent<StageManager>();
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

        // The stage start info.
        public void ApplyStageStartInfo(GameplayInfo.StageStartInfo stageStartInfo)
        {
            // If the information is valid.
            if(stageStartInfo.valid)
            {
                stageName = stageStartInfo.name;
                stageUnitGroups = stageStartInfo.stageUnitGroups;
                difficulty = stageStartInfo.difficulty;
                stageIndex = stageStartInfo.index;
            }
            else
            {
                Debug.LogWarning("The stage info was not marked as valid.");
            }

            // If the stage units list is empty, generate a list of all types.
            if(stageUnitGroups.Count <= 0)
                stageUnitGroups = UnitsInfo.GenerateUnitGroupsList();
    }

        // Goes to the world.
        public void ToWorld()
        {
            // Saves the stage info and goes into the world.
            GameplayInfo.Instance.SaveStageInfo(this);
            ToWorldScene();
        }

        // Goes to the world scene.
        public void ToWorldScene()
        {
            SceneManager.LoadScene(worldScene);
        }
       
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
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