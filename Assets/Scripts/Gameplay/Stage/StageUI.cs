using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using util;
using UnityEditor.SceneManagement;

namespace RM_MST
{
    // The stage UI.
    public class StageUI : GameplayUI
    {
        // The singleton instance.
        private static StageUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The stage manager.
        public StageManager stageManager;

        [Header("Text")]

        // The stage name text.
        public TMP_Text stageNameText;

        // The time text.
        public TMP_Text timeText;

        // The score text.
        public TMP_Text pointsText;

        // The units table.
        public UnitsTable unitsTable;


        [Header("Progress Bars")]

        // The points bar.
        public ProgressBar pointsBar;

        // The damage bar.
        public ProgressBar surfaceHealthBar;

        [Header("PlayerUI")]

        // The conversion text.
        public TMP_Text conversionText;

        // The 7 units buttons.
        public UnitsButton unitsButton0;
        public UnitsButton unitsButton1;
        public UnitsButton unitsButton2;
        public UnitsButton unitsButton3;
        public UnitsButton unitsButton4;
        public UnitsButton unitsButton5;
        public UnitsButton unitsButton6;

        // Randomizes the unit button values if set to 'true'.
        private bool randomizeUnitButtons = false;

        [Header("End Windows")]

        // The game win window.
        public GameObject stageWonWindow;

        // The stage clear time text.
        public TMP_Text stageWonTimeText;

        // The time clear score text.
        public TMP_Text stageWonScoreText;

        // The game lost window.
        public GameObject stageLostWindow;
        // Constructor
        private StageUI()
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

            // Gets the instance.
            if (stageManager == null)
                stageManager = StageManager.Instance;
        }

        // Late start is called on the first frame update.
        protected override void LateStart()
        {
            base.LateStart();

            // Set the name.
            stageNameText.text = stageManager.stageName;

            // Updates all the UI.
            UpdateHUD();
        }

        // Gets the instance.
        public static StageUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<StageUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Stage UI (singleton)");
                        instance = go.AddComponent<StageUI>();
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

        // WINDOWS
        // Closes all the windows.
        public override void CloseAllWindows()
        {
            base.CloseAllWindows();

            stageWonWindow.SetActive(false);
            stageLostWindow.SetActive(false);
        }


        // UI

        // Updates the HUD. This is not counting the unit buttons.
        // TODO: rename this to be more clear?
        public void UpdateHUD()
        {
            UpdateTimeText();
            UpdatePointsText();
            UpdatePointsBar();
            UpdateSurfaceHealthBar();
            UpdateUnitsTable();
        }

        // Updates the time text.
        public void UpdateTimeText()
        {
            timeText.text = StringFormatter.FormatTime(stageManager.stageTime, false, true, false);
        }

        // Updates the points text.
        public void UpdatePointsText()
        {
            pointsText.text = Mathf.Round(stageManager.player.GetPoints()).ToString();
        }

        // Updates the points bar.
        public void UpdatePointsBar()
        {
            // Get the points percentage and clamp it.
            float percent = stageManager.player.GetPoints() / stageManager.pointsGoal;
            percent = Mathf.Clamp01(percent);
            pointsBar.SetValueAsPercentage(percent);
        }

        // Updates the surface health bar.
        public void UpdateSurfaceHealthBar()
        {
            // Get the percent and update the damage bar.
            float percent = stageManager.stageSurface.health / stageManager.stageSurface.maxHealth;
            percent = Mathf.Clamp01(percent);
            surfaceHealthBar.SetValueAsPercentage(percent);
        }

        // Updates the units table with the provied conversion. By default it's set to 'none'.
        public void UpdateUnitsTable(UnitsInfo.unitGroups group = UnitsInfo.unitGroups.none)
        {
            unitsTable.SetGroup(group);
        }

        // GAMEPLAY
        // Set all the unit buttons to be active or inactive.
        public void SetAllUnitButtonsActive(bool active)
        {
            unitsButton1.gameObject.SetActive(active);
            unitsButton2.gameObject.SetActive(active);
            unitsButton3.gameObject.SetActive(active);
            unitsButton4.gameObject.SetActive(active);
            unitsButton5.gameObject.SetActive(active);
            unitsButton6.gameObject.SetActive(active);
            unitsButton0.gameObject.SetActive(active);
        }

        // Makes all unit buttons active.
        public void EnableAllUnitButtons()
        {
            SetAllUnitButtonsActive(true);
        }

        // Makes all unit buttons inactive.
        public void DisableAllUnitButtons()
        {
            SetAllUnitButtonsActive(false);
        }


        // Updates the units buttons.
        public void UpdateConversionAndUnitsButtons(Meteor meteor)
        {
            // Clear all the buttons.
            ClearConversionAndUnitsButtons();

            // No meteor, so return null.
            if (meteor == null)
                return;

            // No conversion, so return null.
            if (meteor.conversion == null)
                return;

            // Conversion
            conversionText.text = stageManager.GenerateConversionQuestion(meteor);

            // If there is a slash, then this is a fraction.
            bool isFraction = conversionText.text.Contains("/");

            // Buttons
            // Makes a list of the unit buttons.
            List<UnitsButton> unitsButtons = new List<UnitsButton>()
            {
                unitsButton0,
                unitsButton1,
                unitsButton2, 
                unitsButton3, 
                unitsButton4, 
                unitsButton5, 
                unitsButton6
            };

            // A stack of the button inactive indexes.
            Stack<int> buttonInactiveIndexes = new Stack<int>();

            // Remove inactive buttons. There should always be at least one button.
            for(int i = 0; i < unitsButtons.Count; i++)
            {
                // If the button is not active and enabled, remove it.
                if(!unitsButtons[i].isActiveAndEnabled)
                {
                    // Add to the stack.
                    buttonInactiveIndexes.Push(i);
                }
            }

            // Don't remove any if all buttons would be removed.
            if(buttonInactiveIndexes.Count != unitsButtons.Count)
            {
                // Remove the relevant indexes.
                while(buttonInactiveIndexes.Count > 0 && unitsButtons.Count > 0)
                {
                    // Gets the index.
                    int index = buttonInactiveIndexes.Pop();

                    // Valid index check.
                    if(index >= 0 && index < unitsButtons.Count)
                        unitsButtons.RemoveAt(index);
                }
            }

            // If there are no buttons left, add in button 4 (the middle button).
            if (unitsButtons.Count == 0)
                unitsButtons.Add(unitsButton4);

            // If the unit buttons should be randomized.
            if (randomizeUnitButtons)
            {
                // The random button list.
                List<UnitsButton> randomButtonList = new List<UnitsButton>();

                // While there are values.
                while(unitsButtons.Count > 0)
                {
                    // Grabs the index, and gets the button.
                    int randIndex = Random.Range(0, unitsButtons.Count);
                    UnitsButton unitsButton = unitsButtons[randIndex];

                    // Adds the button to the random list, and removes it from the normal list.
                    randomButtonList.Add(unitsButton);
                    unitsButtons.Remove(unitsButton);
                }

                // Replace the list with the randomized buttons.
                unitsButtons = new List<UnitsButton>(randomButtonList);
            }


            // Gets set to 'true' if the right value is found.
            float trueOutputValue = meteor.GetConvertedValue();
            bool foundRightValue = false;

            // Goes through all buttons and gets the values.
            for(int i = 0; i < unitsButtons.Count && i < meteor.possibleOutputs.Length; i++)
            {
                // If the question uses fractions, try to display the output as a fraction.
                if(isFraction)
                {
                    // The result will be shown as a fraction if possible.
                    unitsButtons[i].SetMeasurementValueAndSymbol(meteor.possibleOutputs[i], meteor.conversion.GetOutputSymbol(), true);
                }
                else
                {
                    // Sets the text and values.
                    unitsButtons[i].SetMeasurementValueAndSymbol(meteor.possibleOutputs[i], meteor.conversion.GetOutputSymbol());
                }
                

                // If this is the true output value, set that it's been found.
                if (meteor.possibleOutputs[i] == trueOutputValue)
                {
                    foundRightValue = true;
                    unitsButtons[i].correctValue = true;
                }
            }

            // If the right value has not been found, set it to a random button.
            if(!foundRightValue)
            {
                // Gets random indexes for the output and the buttons.
                int outputIndex = Random.Range(0, meteor.possibleOutputs.Length);
                int buttonIndex;

                // If the indexes match, replace it on that button.
                // If they don't match, choose a random button.
                if(outputIndex >= 0 && outputIndex < unitsButtons.Count)
                    buttonIndex = outputIndex;
                else
                    buttonIndex = Random.Range(0, unitsButtons.Count);

                // Replaces the value at the provided index, and marks that button as having the true value.
                meteor.possibleOutputs[outputIndex] = trueOutputValue;
                unitsButtons[buttonIndex].SetMeasurementValue(trueOutputValue);
                unitsButtons[buttonIndex].correctValue = true;
            }
        }

        // Clears the units buttons.
        public void ClearConversionAndUnitsButtons()
        {
            // Conversion
            conversionText.text = "-";

            // Buttons
            unitsButton1.ClearButton();
            unitsButton2.ClearButton();
            unitsButton3.ClearButton();
            unitsButton4.ClearButton();
            unitsButton5.ClearButton();
            unitsButton6.ClearButton();
            unitsButton0.ClearButton();
        }

        // Sets all unit buttons to be interactable.
        public void SetUnitButtonsInteractable(bool interactable)
        {
            unitsButton1.button.interactable = interactable;
            unitsButton2.button.interactable = interactable;
            unitsButton3.button.interactable = interactable;
            unitsButton4.button.interactable = interactable;
            unitsButton5.button.interactable = interactable;
            unitsButton6.button.interactable = interactable;
            unitsButton0.button.interactable = interactable;
        }

        // Set the unit buttons interactable to true.
        public void MakeUnitButtonsInteractable()
        {
            SetUnitButtonsInteractable(true);
        }

        // Set the unit buttons interactable to false.
        public void MakeUnitButtonsUninteractable()
        {
            SetUnitButtonsInteractable(false);
        }



        // SPEED
        // Sets the game to fast.
        public void SetToFastSpeed()
        {
            stageManager.SetToFastSpeed();
        }

        // Toggles fast speed.
        public void ToggleFastSpeed()
        {
            stageManager.ToggleFastSpeed();
        }

        // Sets the game to slow.
        public void SetToSlowSpeed()
        {
            stageManager.SetToSlowSpeed();
        }

        // Toggles slow speed.
        public void ToggleSlowSpeed()
        {
            stageManager.ToggleSlowSpeed();
        }


        // LASER

        // Shoots the laser with the value from the provided value.
        public void ShootLaserShot(float value)
        {
            stageManager.player.ShootLaserShot(value);
        }

        // Shoots the laser with the value from the provided units button.
        public void ShootLaserShot(UnitsButton unitsButton)
        {
            // If the button is automatically correct, pull the exact value.
            if(unitsButton.correctValue)
            {
                // Gets the meteor.
                Meteor meteor = stageManager.meteorTarget.GetMeteor();

                // Use meteor's value if true. Use button's value if false.
                if (meteor != null)
                    stageManager.player.ShootLaserShot(meteor.GetConvertedValue());
                else
                    stageManager.player.ShootLaserShot(unitsButton.GetMeasurementValue());
            }
            else // Button is not automatically correct.
            {
                stageManager.player.ShootLaserShot(unitsButton.GetMeasurementValue());
            }
            
        }


        // STAGE END
        // Updates the HUD one last time after the stage ends.
        public void OnStageEnd()
        {
            UpdateHUD();
        }

        // Called when the stage has been won.
        public void OnStageWon()
        {
            CloseAllWindows();
            stageWonWindow.SetActive(true);

            // Set the time and score text.
            stageWonTimeText.text = StringFormatter.FormatTime(stageManager.stageTime, false, true, false);
            stageWonScoreText.text = stageManager.stageFinalScore.ToString(); // Already calculated.
        }

        // Called when the stage has been lost.
        public void OnStageLost()
        {
            CloseAllWindows();
            stageLostWindow.SetActive(true);
        }

        // Called to restart the stage.
        public void RestartStage()
        {
            stageManager.ResetStage();
        }

        // Called when the start has been restarted.
        public void OnStageReset()
        {
            CloseAllWindows();
            ClearConversionAndUnitsButtons();
        }

        // Goes to the world.
        public void ToWorld()
        {
            stageManager.ToWorld();
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