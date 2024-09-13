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

        // The renderer of the stage art.
        public Image stageRenderer;

        // The background of the UI.
        public Image background;

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

            // TODO: play the button SFX for the stage UI?
            // // Plays the button SFX upon being activated.s
            // if (manager.worldAudio != null)
            //     manager.worldAudio.PlayButtonSfx();

        }

        // TODO: add clear challenger option?
        // Sets the challenger.
        public void SetStageWorld(StageWorld stageWorld, int index)
        {
            // Sets the stage and the index.
            this.stageWorld = stageWorld;
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