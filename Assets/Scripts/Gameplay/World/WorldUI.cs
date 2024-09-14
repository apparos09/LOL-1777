using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The world UI.
    public class WorldUI : GameplayUI
    {
        // The singleton instance.
        private static WorldUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("WorldUI")]
        // The world manager.
        public WorldManager worldManager;

        [Header("WorldUI/StageUI")]
        // The stage world UI.
        public StageWorldUI stageWorldUI;

        [Header("WorldUI/Windows")]

        // The units info window.
        public UnitsInfoMenu unitsInfoMenu;

        // The save window.
        public GameObject saveWindow;

        // Constructor
        private WorldUI()
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
        protected override void Start()
        {
            base.Start();

            // Sets the world manager.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // Sets the tutorial isntance.
            if (tutorialUI == null)
                tutorialUI = TutorialUI.Instance;

            // Hide the stage world UI.
            HideStageWorldUI(true);
        }

        // Gets the instance.
        public static WorldUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<WorldUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("World UI (singleton)");
                        instance = go.AddComponent<WorldUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // WINDOWS //
        // Checks if a window is open.
        public override bool IsWindowOpen()
        {
            // Gets the windows.
            List<GameObject> windows = new List<GameObject>
            {
                gameSettingsUI.gameObject,
                unitsInfoMenu.gameObject,
                saveWindow
            };

            // Only checks the settings window here.
            bool open = false;

            // Goes through all windows.
            foreach(GameObject window in windows)
            {
                // Window is active.
                if(window.activeSelf)
                {
                    open = true;
                    break;
                }
            }

            // Result.
            return open;
        }

        // Closes all the windows.
        public override void CloseAllWindows()
        {
            // Close windows in this script.
            unitsInfoMenu.gameObject.SetActive(false);
            saveWindow.SetActive(false);

            base.CloseAllWindows();
        }


        // STAGE UI
        // Returns 'true' if the stage world UI is active.
        public bool IsStageWorldUIActive()
        {
            return stageWorldUI.isActiveAndEnabled;
        }

        // Show the stage world UI.
        public void ShowStageWorldUI(StageWorld stageWorld, int index)
        {
            stageWorldUI.SetStageWorld(stageWorld, index);
            stageWorldUI.gameObject.SetActive(true);
        }

        // Hide the stage world UI.
        public void HideStageWorldUI(bool clearUI)
        {
            // If the UI should be cleared.
            if(clearUI)
            {
                stageWorldUI.ClearStageWorld();
            }

            // Hids the stage world UI.
            stageWorldUI.gameObject.SetActive(false);
        }

        // Starts the stage in the world UI.
        public void StartStage(StageWorld stage)
        {
            worldManager.ToStage(stage);
        }

        // Rejects the stage in the world UI.
        public void RejectStage()
        {
            HideStageWorldUI(true);
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