using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_MST
{
    // The UI for the a stage to be selected from the world.
    public class StageWorldUI : MonoBehaviour
    {
        // The world manager.
        public WorldManager worldManager;

        // The world UI.
        public WorldUI worldUI;

        // The world stage being displayed.
        public StageWorld stageWorld;

        // The index of the challenger.
        public int stageWorldIndex = -1;

        [Header("Images")]

        // The background of the UI.
        public Image background;

        // The renderer of the stage art.
        public Image stageRenderer;

        [Header("Text")]

        // The stage name text.
        public TMP_Text stageNameText;

        // The stage description text.
        public TMP_Text stageDescText;

        // TODO: add stage difficulty text?

        [Header("Buttons")]

        // The stage accept button.
        public Button startButton;

        // The stage reject button.
        public Button rejectButton;


        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        void Start()
        {
            // Sets the world manager.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // Sets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            // May not be needed.
            // EnableButtons();

            // Grabs the instance.
            if(worldManager == null)
                worldManager = WorldManager.Instance;

            // Sets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;

            // The stage world UI has been opened.
            OnStageWorldUIOpened();

            // TODO: play the button SFX for the stage UI?
            // // Plays the button SFX upon being activated.s
            // if (manager.worldAudio != null)
            //     manager.worldAudio.PlayButtonSfx();

        }

        // TODO: add clear challenger option?
        // Sets the challenger.
        public void SetStageWorld(StageWorld newStageWorld, int index)
        {
            // Sets the stage and the index.
            stageWorld = newStageWorld;
            stageWorldIndex = index;

            // Updates the UI.
            UpdateUI();
        }

        // Clears the stage world information.
        public void ClearStageWorld()
        {
            stageWorld = null;
            stageWorldIndex = -1;

            // Updates the UI.
            UpdateUI();
        }

        // UI
        // Updates the UI.
        public void UpdateUI()
        {
            UpdateStageSprite();
            UpdateStageNameText();
            UpdateStageDescriptionText();
        }

        // Updates the Challenger Sprite
        public void UpdateStageSprite()
        {
            if(stageWorld != null) // Set sprite.
            {
                stageRenderer.sprite = stageWorld.stageSprite;
            }
            else // Clear sprite.
            {
                stageRenderer.sprite = null;
            }
            
        }

        // Updates the name text.
        public void UpdateStageNameText()
        {
            // Checks if the stage name is set.
            if (stageWorld != null)
                stageNameText.text = stageWorld.stageName;
            else
                stageNameText.text = "-";
        }

        // Updates the stage description text.
        public void UpdateStageDescriptionText()
        {
            // Checks if the stage description is set.
            if (stageWorld != null)
                stageDescText.text = stageWorld.stageDesc;
            else
                stageDescText.text = "-";
        }

        // Called when the stage world UI has been opened.
        public void OnStageWorldUIOpened()
        {
            // If the stage world is not equal to none.
            if (stageWorld != null)
            {
                // If the tutorial is being used.
                if(worldManager.IsUsingTutorial() && worldManager.tutorials != null)
                {
                    // Gets the tutorials object.
                    Tutorials tutorials = worldManager.tutorials;

                    // If there are multiple unit groups, try loading the mix stage tutorial.
                    if(stageWorld.unitGroups.Count > 1)
                    {
                        // Loads the mix stage tutorial if it hasn't been used.
                        if(!tutorials.clearedMixStageTutorial)
                        {
                            tutorials.LoadMixStageTutorial();
                        }
                    }
                    else if(stageWorld.unitGroups.Count == 1)
                    {
                        // Gets the group.
                        UnitsInfo.unitGroups group = stageWorld.unitGroups[0];

                        // Checks the group to see what intro should be loaded.
                        switch (group)
                        {
                            case UnitsInfo.unitGroups.lengthImperial:

                                // Tutorial not cleared, load it.
                                if (!tutorials.clearedLengthImperialTutorial)
                                    tutorials.LoadLengthImperialTutorial();

                                break;

                            case UnitsInfo.unitGroups.weightImperial:

                                // Tutorial not cleared, load it.
                                if (!tutorials.clearedWeightImperialTutorial)
                                    tutorials.LoadWeightImperialTutorial();

                                break;

                            case UnitsInfo.unitGroups.time:

                                // Tutorial not cleared, load it.
                                if (!tutorials.clearedTimeTutorial)
                                    tutorials.LoadTimeTutorial();

                                break;

                            case UnitsInfo.unitGroups.lengthMetric:

                                // Tutorial not cleared, load it.
                                if (!tutorials.clearedLengthMetricTutorial)
                                    tutorials.LoadLengthMetricTutorial();

                                break;

                            case UnitsInfo.unitGroups.weightMetric:

                                // Tutorial not cleared, load it.
                                if (!tutorials.clearedWeightMetricTutorial)
                                    tutorials.LoadWeightMetricTutorial();

                                break;

                            case UnitsInfo.unitGroups.capacity:

                                // Tutorial not cleared, load it.
                                if (!tutorials.clearedCapacityTutorial)
                                    tutorials.LoadCapacityTutorial();

                                break;
                        }
                    }
                }
            }
        }

        // Start/Reject
        // Accepts the challenge.
        public void StartStage()
        {
            // If the world is set.
            if(stageWorld != null)
                worldUI.StartStage(stageWorld);
        }

        // Declines the challenge.
        public void RejectStage()
        {
            worldUI.RejectStage();
        }

        // Enables the buttons for the challenger UI.
        public void EnableButtons()
        {
            startButton.interactable = true;
            rejectButton.interactable = true;
        }

        // Disables the buttons for the challenger UI.
        public void DisableButtons()
        {
            startButton.interactable = false;
            rejectButton.interactable = false;
        }
    }
}