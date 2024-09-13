using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_MST
{
    // A challenger that's encountered in the game world.
    // The 'World' part is added to make it clearer that it belongs to the world area, not the match area.
    public class StageWorld : MonoBehaviour
    {
        // World manager.
        public WorldManager manager;

        // The collider for the challenger.
        public new BoxCollider2D collider;

        // The sprite renderer for the the challenger.
        public SpriteRenderer spriteRenderer;

        // The name text.
        public TMP_Text nameText;

        // The stage sprite.
        public Sprite stageSprite;

        [Header("Info")]

        // The stage name.
        public string stageName = "";

        // The stage description.
        public string stageDesc = "";

        // The units types for the stage.
        public List<UnitsInfo.units> unitTypes = new List<UnitsInfo.units>();

        // The difficulty of the stage.
        public int difficulty = 0;

        // Gets set to 'true' when the stage has been cleared.
        // TODO: make this private when not testing.
        public bool cleared = false;

        // Shows if the stage is available.
        private bool available = true;

        // Start is called before the first frame update
        void Start()
        {
            // Manager.
            if (manager == null)
                manager = WorldManager.Instance;

            // Checks for the collider.
            if (collider == null)
            {
                // Tries to get the collider (no longer checks children for misinput concerns).
                collider = GetComponent<BoxCollider2D>();
            }

            // Checks for the sprite renderer.
            if (spriteRenderer == null)
            {
                // Tries to get the component (no longer checks children for misinput concerns).
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            // Sets the stage text.
            if(nameText != null)
                nameText.text = stageName;

            // If there is no description, generate one.
            if(stageDesc == "")
                GenerateStageDescription();
        }

        // MouseDown
        private void OnMouseDown()
        {
            // Grabs the instance if it's not set.
            if (manager == null)
                manager = WorldManager.Instance;

            // Show the challenger prompt if no window is open, and if the tutorial text box isn't open.
            if(!manager.worldUI.IsWindowOpen() && !manager.worldUI.IsTutorialTextBoxOpen())
            {
                // Shows the challenge UI.
                ShowStageWorldUI();
            }
            
        }

        // Returns 'true' if the stage is available.
        public bool IsStageAvailable()
        {
            return available;
        }

        // Sets if the stage is available or not.
        public void SetStageAvailable(bool avail, bool playAnim = true)
        {
            available = avail;

            // Checks if the stage is available.
            if (available) // Available
            {
                collider.enabled = true;
                spriteRenderer.enabled = true;
            }
            else // Unavailable.
            {
                collider.enabled = false;
                spriteRenderer.enabled = false;
            }

            // TODO: implement animation.
        }

        // Sets the stage to be available.
        public void SetStageToAvailable(bool playAnim = true)
        {
            SetStageAvailable(true, playAnim);
        }

        // Sets the stage to be unavailable.
        public void SetStageToUnavailable(bool playAnim = true)
        {
            SetStageAvailable(false, playAnim);
        }

        // Checks if the stage has been cleared.
        public bool IsStageCleared()
        {
            return cleared;
        }

        // Sets if the stage is cleared.
        public void SetStageCleared(bool clear)
        {
            cleared = clear;

            // Checks if the sprite renderer is enabled.
            bool rendererEnabled = spriteRenderer.enabled;

            // Changes the colour based on if the stage is cleared or not.
            if (cleared && spriteRenderer.color != Color.grey) // Greyed Out
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = Color.grey;
                spriteRenderer.enabled = rendererEnabled;
            }
            else if(!cleared && spriteRenderer.color != Color.white) // Regular Colour
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = Color.white;
                spriteRenderer.enabled = rendererEnabled;
            }
        }

        // Returns 'true' if the stage is available and cleared.
        public bool IsStageAvailableAndCleared()
        {
            return available && cleared;
        }

        // Generates the stage world description.
        public string GenerateStageDescription()
        {
            // TODO: generate a stage description based on the units type.
            return stageDesc;
        }

        // Tries to show the challenge UI and loads in the content.
        public void ShowStageWorldUI()
        {
            // Checks if the stage is available.
            if(available)
            {
                // Checks if the stage has been cleared yet. If not, allow the challenge.
                if(!cleared)
                {
                    // TODO: don't do this if the UI is active?

                    // Shows the selected stage.
                    manager.worldUI.ShowStageWorldUI(this, manager.GetStageWorldIndex(this));
                }    
            }
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}