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
        public WorldManager worldManager;

        // The collider for the challenger.
        public new BoxCollider2D collider;

        // The sprite renderer for the the challenger.
        public SpriteRenderer spriteRenderer;

        // The name text.
        public TMP_Text nameText;

        // The stage sprite.
        public Sprite stageSprite;

        // Gets set to 'true' when late start has been called.
        private bool calledLateStart = false;

        [Header("Info")]

        // The stage name.
        public string stageName = "";

        // The stage name key.
        public string stageNameKey = "";

        // If 'true', the name text is automatically set.
        public bool autoSetNameText = true;

        // The stage description.
        public string stageDesc = "";

        // The stage description key.
        public string stageDescKey = "";

        // The units types for the stage.
        public List<UnitsInfo.unitGroups> unitGroups = new List<UnitsInfo.unitGroups>();

        // The difficulty of the stage.
        public int difficulty = 0;

        // The number of loses on this stage.
        public int losses = 0;

        // Gets set to 'true' when the stage has been cleared.
        // TODO: make this private when not testing.
        public bool cleared = false;

        // Shows if the stage is available.
        private bool available = true;

        // Start is called before the first frame update
        void Start()
        {
            // Manager.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

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
        }

        // Called on the first update frame.
        void LateStart()
        {
            UnitsInfo unitsInfo = UnitsInfo.Instance;

            // NAME
            // If the stage name and the key are empty, generate them.
            if (stageName == "" && stageNameKey == "")
            {
                // Sets the name.
                stageName = GenerateStageName();

                // Sets the speak key to the first group.
                if (unitGroups.Count > 0)
                    stageNameKey = UnitsInfo.GetUnitsGroupNameKey(unitGroups[0]);
            }
            else
            {
                // If the name key is not empty, translate the name.
                if(stageNameKey != "")
                {
                    if(LOLManager.IsLOLSDKInitialized())
                        stageName = LOLManager.Instance.GetLanguageText(stageNameKey);
                }
            }

            // Sets the stage text to the stage name.
            if (nameText != null && autoSetNameText)
            {
                nameText.text = stageName;
            }

            // DESCRIPTION
            // If the stage description and the key are empty, generate them.
            if (stageDesc == "" && stageDescKey == "")
            {
                // Sets the description.
                stageDesc = GenerateStageDescription();

                // Sets the speak key to the first group.
                if (unitGroups.Count > 0)
                    stageDescKey = UnitsInfo.GetUnitsGroupDescriptionKey(unitGroups[0]);
            }
            else
            {
                // If the description key is not empty, translate the name.
                if (stageDescKey != "")
                {
                    if (LOLManager.IsLOLSDKInitialized())
                        stageDesc = LOLManager.Instance.GetLanguageText(stageDescKey);
                }
            }

            // Late Start has been called.
            calledLateStart = true;
        }

        // MouseDown
        private void OnMouseDown()
        {
            // Grabs the instance if it's not set.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // Show the challenger prompt if no window is open, and if the tutorial text box isn't open.
            if(!worldManager.worldUI.IsWindowOpen() && !worldManager.worldUI.IsTutorialTextBoxOpen())
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

        // Generates the info based on the type.
        // 1 = Name, 2 = Description
        private string GenerateStageInfo(int type)
        {
            // The temporary string.
            string str = "";

            // The units info.
            UnitsInfo unitsInfo = UnitsInfo.Instance;

            // The used groups.
            List<UnitsInfo.unitGroups> usedGroups = new List<UnitsInfo.unitGroups>();

            // TODO: you should probably have a better way to do this.
            // Goes through all the unit groups.
            for (int i = 0; i < unitGroups.Count; i++)
            {
                // If the group hasn't been used yet.
                if (!usedGroups.Contains(unitGroups[i]))
                {
                    switch(type)
                    {
                        default:
                        case 0:
                        case 1: // Name
                            str += unitsInfo.GetUnitsGroupName(unitGroups[i]);
                            break;

                        case 2: // Description
                            str += unitsInfo.GetUnitsGroupDescription(unitGroups[i]);
                            break;
                    }
                    
                    str += ", ";

                    // Add to the used groups.
                    usedGroups.Add(unitGroups[i]);
                }
            }


            // Group was used.
            if(unitGroups.Count > 0)
            {
                // Remove the last two characters to get rid of the comma and space.
                if (str.Length > 2)
                    str = str.Remove(str.Length - 2, 2);
            }
            

            // TODO: just say "all" if all groups are shown.
            return str;
        }

        // Generates the stage world name. This does not set the name.
        public string GenerateStageName()
        {
            return GenerateStageInfo(1);
        }

        // Generates the stage world description. This does not set the description.
        public string GenerateStageDescription()
        {
            return GenerateStageInfo(2);
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
                    worldManager.worldUI.ShowStageWorldUI(this, worldManager.GetStageWorldIndex(this));
                }    
            }
        }

        // Generates the stage start information.
        public GameplayInfo.StageStartInfo GenerateStageInfo()
        {
            // The stage start info.
            GameplayInfo.StageStartInfo stageStartInfo = new GameplayInfo.StageStartInfo();

            // Sets the name, units groups, and difficulty.
            stageStartInfo.name = stageName;
            stageStartInfo.stageUnitGroups = unitGroups;
            stageStartInfo.difficulty = difficulty;
            stageStartInfo.losses = losses;

            // Gets the index for the stage start.
            stageStartInfo.index = worldManager.GetStageWorldIndex(this);

            // The info is valid to read from.
            stageStartInfo.valid = true;

            // Reutnrs the object.
            return stageStartInfo;
        }

        // Update is called once per frame
        void Update()
        {
            // Calls late start.
            if (!calledLateStart)
                LateStart();
        }
    }
}