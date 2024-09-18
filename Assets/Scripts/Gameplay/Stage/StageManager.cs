using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        [Header("StageManager")]
        // The stage user interface.
        public StageUI stageUI;

        // The stage.
        public Stage stage;

        // The stage's name.
        public string stageName;

        // The stage index.
        public int stageIndex = -1;

        // The difficulty.
        // TODO: make private.
        public int difficulty = 0;

        // The base difficulty for the game.
        public int baseDifficulty = 0;

        // The maximum difficulty.
        public const int DIFFICULTY_MAX = 9;

        // Adjusts the difficulty dynamically by the number of losses the player has.
        private bool dynamicDifficulty = true;

        // The total number of losses.
        public int losses = 0;

        // The phase for the game.
        // TODO: make private.
        public int phase = 0;

        // The maximum phase value.
        public const int PHASE_MAX = 4;

        // The score that must be met to win the game.
        public float pointsGoal = 1000.0F;

        // The player for the stage.
        public PlayerStage player;

        // The target for the meteor.
        public MeteorTarget meteorTarget;

        // The stage surface.
        public StageSurface stageSurface;

        // The barriers for the stage.
        public List<Barrier> stageBarriers;

        // The timer for the stage.
        public float stageTime = 0.0F;

        // The fast game time scale.
        private float FAST_GAME_TIME_SCALE = 2.0F;

        // The slow game time scale.
        private float SLOW_GAME_TIME_SCALE = 0.5F;

        // The final score for the stage.
        public float stageFinalScore = 0.0F;

        // Shows if the stage is cleared.
        public bool cleared = false;

        // The world scene.
        public string worldScene = "WorldScene";

        // Gets set to 'true' when the game is running.
        private bool runningGame = false;

        [Header("Conversions")]
        // The units used for the stage.
        public List<UnitsInfo.unitGroups> stageUnitGroups = new List<UnitsInfo.unitGroups>();

        // The conversions for the stage.
        public List<UnitsInfo.UnitsConversion> conversions = new List<UnitsInfo.UnitsConversion>();

        // The minimum units input value.
        public const float UNITS_INPUT_VALUE_MIN = 0.0F;

        // The maximum units input value.
        public const float UNITS_INPUT_VALUE_MAX = 1000.0F;

        // The number of decimal places for the units.
        // TODO: maybe limit to 2 decimal places.
        public const int UNITS_DECIMAL_PLACES = 3;

        // If 'true', random inputs are limited to whole numbers for non-metric units.
        private bool limitRandomUnitInputs = true;

        // If 'true', fractions are used in the game.
        private bool useFractions = true;

        // The fraction display chance.
        public const float FRACTION_DISPLAY_CHANCE = 0.5F;

        [Header("Meteors")]
        // TODO: make private.
        // The meteor spawn rate.
        public float meteorSpawnRate = 1.0F;

        // The timer used for spawning meteors.
        private float meteorSpawnTimer = 0.0F;

        // TODO: make private.
        // The meteor fall speed factor.
        public float meteorSpeedMax = 1.0F;

        // The meteor prefabs.
        public List<Meteor> meteorPrefabs = new List<Meteor>();

        // TODO: The meteor pool. May not need this.
        // If you use this, make sure to account for the FindAndDestroyAllActiveMeteors function.
        // private List<Meteor> meteorPool = new List<Meteor>();

        // The total number of meteors that can be active at once.
        private int ACTIVE_METEORS_COUNT_MAX = 12;



        [Header("Combo")]
        // The combo for the stage.
        public int combo = 0;

        // The highest combo achieved.
        public int highestCombo = 0;

        // The timer for triggering a combo.
        private float comboTimer = 0;

        // The maximum time for the combo (in seconds).
        private const float COMBO_TIMER_MAX = 5.0F;

        // The combo display.
        public ComboDisplay comboDisplay;

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

            // If the barrier list is empty, find all the barriers.
            if (stageBarriers.Count == 0)
            {
                stageBarriers = new List<Barrier>(FindObjectsOfType<Barrier>());
            }

            // Just fill the stage name with elipses if there is no name.
            if (stageName == "")
                stageName = "...";

            // Sets the difficulty 
            SetDifficulty(difficulty, true);

            // If the difficulty should be dynamically adjusted.
            if(dynamicDifficulty)
                AdjustDifficultyByLosses();
        }

        // The function called after the start function.
        protected override void LateStart()
        {
            base.LateStart();

            // This is done here to make sure that the unit info object has been loaded.

            // Getting the conversions.
            conversions = new List<UnitsInfo.UnitsConversion>();
            List<UnitsInfo.unitGroups> usedGroups = new List<UnitsInfo.unitGroups>();


            // Goes through all the stage unit groups.
            foreach (UnitsInfo.unitGroups group in stageUnitGroups)
            {
                // If the group hasn't been used yet, get the group.
                if(!usedGroups.Contains(group))
                {
                    conversions.AddRange(UnitsInfo.Instance.GetGroupConversionListCopy(group));
                    usedGroups.Add(group);
                }
            }

            // Closes all the windows, and clears the buttons.
            stageUI.CloseAllWindows();
            stageUI.ClearConversionAndUnitsButtons();

            // The game is now running.
            runningGame = true;
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
                losses = stageStartInfo.losses;
                stageIndex = stageStartInfo.index;
            }
            else
            {
                Debug.LogWarning("The stage info was not marked as valid.");
            }

            // If the stage units list is empty, generate a list of all types.
            if(stageUnitGroups.Count <= 0)
                stageUnitGroups = UnitsInfo.GenerateUnitGroupsList();

            // Sets the difficulty using the proper function.
            SetDifficulty(difficulty);
        }

        // Gets the difficulty.
        public int GetDifficulty()
        {
            return difficulty;
        }

        // Sets the difficulty for the game.
        public void SetDifficulty(int difficultyLevel, bool setBaseDifficulty = true)
        {
            // Sets the difficulty.
            difficulty = Mathf.Clamp(difficultyLevel, 1, DIFFICULTY_MAX);
            
            // If the base difficulty should be set, set it.
            if(setBaseDifficulty)
                baseDifficulty = difficulty;


            // // Changes parameters based on the difficulty.
            // // TODO: implement.
            // switch(difficulty)
            // {
            //     default:
            //     case 0:
            //     case 1:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            // 
            //     case 2:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            //     
            //     case 3:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            //     
            //     case 4:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            //     
            //     case 5:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            //     
            //     case 6:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            // 
            //     case 7:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            // 
            //     case 8:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            // 
            //     case 9:
            //         meteorSpawnRate = 1.0F;
            //         meteorSpeedFactor = 1.0F;
            //         break;
            // 
            // }

            // TODO: activate/deactive certain buttons.
        }

        // Adjusts the difficulty by the amount of losses.
        public void AdjustDifficultyByLosses()
        {
            // For every 3 losses, lower the difficulty by 1.
            int quotient = losses / 3;

            // Set the difficulty with the base difficulty as a basis.
            SetDifficulty(baseDifficulty - quotient, false);
        }

        // Gets the phase.
        public int GetPhase()
        {
            return phase;
        }

        // Sets the phase
        public void SetPhase(int newPhase)
        {
            phase = Mathf.Clamp(newPhase, 1, PHASE_MAX);
        }

        // Sets the game phase by the game progress.
        public void SetPhaseByPlayerPointsProgress()
        {
            // Gets the percent.
            float percent = GetPlayerPointsProgress();

            // Checks the percent.
            if(percent > 0.75F) // Phase 4
            {
                SetPhase(4);
            }
            else if(percent > 0.50F) // Phase 3
            {
                SetPhase(3);
            }
            else if(percent > 0.25F) // Phase 3
            {
                SetPhase(2);
            }
            else // Phase 1
            {
                SetPhase(1);
            }
        }


        // METEORS
        // Spawns a meteor.
        public Meteor SpawnMeteor()
        {
            // No meteor prefabs.
            if(meteorPrefabs.Count == 0)
            {
                Debug.LogError("No meteor prefabs available.");
                return null;
            }

            // Gets a copy of the meteors active list.
            List<Meteor> meteorsActive = Meteor.GetMeteorsActiveListCopy();

            // Maximum meteor count met.
            if (meteorsActive.Count >= ACTIVE_METEORS_COUNT_MAX)
            {
                // Debug.LogWarning("Maximum meteor count met.");
                return null;
            }

            // Generates a meteor.
            int randomIndex = Random.Range(0, meteorPrefabs.Count);
            Meteor meteor = Instantiate(meteorPrefabs[randomIndex]);

            // Generates a random conversion for the meteor, and generates alternate outputs.
            meteor.conversion = GenerateRandomConversionFromList();
            SetRandomConversionInputValue(meteor.conversion);
            meteor.GenerateAlternateOutputs();
            unitsInfo.gameObject.SetActive(true);

            // Set the parent.
            if(stage.meteorParent != null)
                meteor.transform.parent = stage.meteorParent.transform;

            // Generate the spawn point.
            meteor.spawnPoint = stage.GenerateMeteorSpawnPoint();

            // Called when the meteor has spawned.
            meteor.OnSpawn();

            // Add to the list.
            meteorsActive.Add(meteor);

            // Return the meteor.
            return meteor;
        }

        // Called when a meteor is destroyed.
        public void OnMeteorKilled(Meteor meteor)
        {
            // ...
        }

        // Called when a meteor fails to be destroyed.
        public void OnMeteorSurivived(Meteor meteor)
        {
            // Reset the combo.
            combo = 0;
            comboTimer = 0;
        }


        // Gets the closest meteor from the active list.
        public Meteor GetClosestMeteor()
        {
            // The meteor and the distance.
            Meteor meteor = null;
            float meteorDist = 0;

            // Gets a copy of the meteors active list.
            List<Meteor> meteorsActive = Meteor.GetMeteorsActiveListCopy();

            // Goes through all meteors from end to start.
            for (int i = meteorsActive.Count - 1; i >= 0; i--)
            {
                // If the index is null, remove it.
                // This isn't necessary anymore, but I don't feel like removing it.
                if (meteorsActive[i] == null)
                {
                    meteorsActive.RemoveAt(i);
                }
                else // Check the position.
                {
                    // If there is no meteor, track it by default.
                    if(meteor == null)
                    {
                        // Get the meteor and the distance.
                        meteor = meteorsActive[i];

                        // Adjusts the distance to ignore the x and z components.
                        Vector3 meteorAdjustPos = meteor.transform.position;
                        meteorAdjustPos.x = stageSurface.transform.position.x;
                        meteorAdjustPos.z = stageSurface.transform.position.z;

                        // Save the distance.
                        meteorDist = Vector3.Distance(meteorAdjustPos, stageSurface.gameObject.transform.position);
                    }
                    else // Compare distances.
                    {
                        // Grabs the two meteors.
                        Meteor m1 = meteor;
                        Meteor m2 = meteorsActive[i];

                        // The adjusted positions for m2.
                        // They all have the same x and z pos as the surface for this comparison.
                        // M2
                        Vector3 m2AdjustPos = m2.transform.position;
                        m2AdjustPos.x = stageSurface.transform.position.x;
                        m2AdjustPos.z = stageSurface.transform.position.z;

                        // Gets the distances.
                        float m1Dist = meteorDist;
                        float m2Dist = Vector3.Distance(m2AdjustPos, stageSurface.gameObject.transform.position);

                        // Meteor 1 is Closer
                        if(m1Dist < m2Dist)
                        {
                            meteor = m1;
                            meteorDist = m1Dist;
                        }
                        // Meteor 2 is Closer
                        else if(m1Dist > m2Dist)
                        {
                            meteor = m2;
                            meteorDist = m2Dist;
                        }
                    }
                }
            }

            // Returns the meteor.
            return meteor;
        }

        // Refreshes the meteors active list to remove null values.
        public void RefreshMeteorsActiveList()
        {
            Meteor.RefreshMeteorsActiveList();
        }

        // Destroys all the meteors in the list. Some meteors may not be in the list for some reason.
        public void KillAllMeteorsInList()
        {
            Meteor.KillAllMeteorsInActiveList();
        }

        // Finds and kills all meteors. This is more accurate than KillAllMeteorsActive().
        public void FindAndKillAllMeteors()
        {
            // Gets all the meteors.
            Meteor[] meteors = FindObjectsOfType<Meteor>(true);

            // Kills all the meteors.
            foreach(Meteor meteor in meteors)
            {
                meteor.Kill();
            }
        }

        // Generates a units conversion. This isn't needed anymore.
        public UnitsInfo.UnitsConversion GenerateRandomConversionClass()
        {
            // The conversion to be returned.
            UnitsInfo.UnitsConversion conversion;

            // The group the units are part of.
            UnitsInfo.unitGroups group = (UnitsInfo.unitGroups)Random.Range(0, UnitsInfo.UNIT_GROUPS_COUNT);

            // The group.
            switch(group)
            {
                default:
                case UnitsInfo.unitGroups.none:
                    conversion = null;
                    break;

                case UnitsInfo.unitGroups.lengthImperial: // Length
                case UnitsInfo.unitGroups.lengthMetric:
                    conversion = new UnitsInfo.LengthConversion();
                    break;

                case UnitsInfo.unitGroups.weightImperial: // Weight
                case UnitsInfo.unitGroups.weightMetric:
                    conversion = new UnitsInfo.WeightConversion();
                    break;

                case UnitsInfo.unitGroups.time: // Time
                    conversion = new UnitsInfo.TimeConversion();
                    break;
                
                case UnitsInfo.unitGroups.capacity: // Capacity
                    conversion = new UnitsInfo.CapacityConversion();
                    break;
            }

            // Conversion is unknown.
            if (conversion == null)
                return conversion;

            // Sets the group.
            conversion.group = group;

            // Returns the conversion.
            return conversion;
        }

        // Generates a random conversion from the list.
        public UnitsInfo.UnitsConversion GenerateRandomConversionFromList()
        {
            // Conversion object.
            UnitsInfo.UnitsConversion conversion;

            // Checks if there are conversions to pick from.
            if (conversions.Count > 0)
            {
                // The original conversion.
                // These conversions are copied as to prevent the original conversion objects from being altered.
                UnitsInfo.UnitsConversion origConvert = conversions[Random.Range(0, conversions.Count)];

                // Checks the conversion type.
                if(origConvert is UnitsInfo.WeightConversion) // Weight
                {
                    conversion = new UnitsInfo.WeightConversion((UnitsInfo.WeightConversion)origConvert);
                }
                else if(origConvert is UnitsInfo.LengthConversion) // Length
                {
                    conversion = new UnitsInfo.LengthConversion((UnitsInfo.LengthConversion)origConvert);
                }
                else if(origConvert is UnitsInfo.TimeConversion) // Time
                {
                    conversion = new UnitsInfo.TimeConversion((UnitsInfo.TimeConversion)origConvert);
                }
                else if(origConvert is UnitsInfo.CapacityConversion) // Capacity
                {
                    conversion = new UnitsInfo.CapacityConversion((UnitsInfo.CapacityConversion)origConvert);
                }
                else
                {
                    conversion = null;
                }


            }
            else // No conversions, so generate an empty conversion.
            {
                // conversion = GenerateRandomConversionClass();
                conversion = null;
            }

            // Returns the conversion.
            return conversion;
        }

        // Generates a random input value for the conversion.
        public float SetRandomConversionInputValue(UnitsInfo.UnitsConversion conversion)
        {
            // The conversion object doesn't exist.
            if(conversion == null)
            {
                Debug.LogWarning("The conversion object is null. Returning 0.");
                return 0.0F;
            }

            // Generates the value, and gets the factor for the number of decimal palces.
            float value = Random.Range(UNITS_INPUT_VALUE_MIN, UNITS_INPUT_VALUE_MAX);

            // Round the value, and cap it at 3 decimal places.
            value = util.CustomMath.Round(value, UNITS_DECIMAL_PLACES);

            // If random unit inputs should be limited.
            if(limitRandomUnitInputs)
            {
                // If these are not metric units, round up to a whole number.
                if(!UnitsInfo.IsMetricUnits(conversion.group))
                {
                    value = Mathf.Ceil(value);
                }
            }

            // Set and return the value.
            conversion.inputValue = value;
            return value;
        }

        // Gets the meteor spawn rate, modified by the phase.
        public float GetModifiedMeteorSpawnRate()
        {
            // The modifier value.
            float mod;

            // Checks the speed for the spawn rate modifier.
            switch(phase)
            {
                default:
                case 1:
                    mod = 1.0F;
                    break;

                case 2:
                    mod = 0.95F;
                    break;

                case 3:
                    mod = 0.90F;
                    break;

                case 4:
                    mod = 0.85F;
                    break;

            }

            // Get the result.
            float result = meteorSpawnRate * mod;

            // Return the result.
            return result;

        }

        // Gets the meteor speed, modified by the phase.
        public float GetModifiedMeteorSpeedMax()
        {
            // The modifier value.
            float mod;

            // Checks the phase to see what speed to use.
            switch (phase)
            {
                default:
                case 1:
                    mod = 1.0F;
                    break;

                case 2:
                    mod = 1.10F;
                    break;

                case 3:
                    mod = 1.20F;
                    break;

                case 4:
                    mod = 1.30F;
                    break;

            }

            // Get the result.
            float result = meteorSpeedMax * mod;

            // Return the result.
            return result;
        }

        // SPEED
        // Gets the game speed.
        public float GetGameSpeed()
        {
            // Returns the time scale.
            return GetGameTimeScale();
        }

        // Sets the game speed.
        public void SetGameSpeed(float timeScale)
        {
            // Sets the time scale.
            SetGameTimeScale(timeScale);
        }

        // Returns 'true' if the time scale is normal.
        public bool IsNormalSpeed()
        {
            return IsGameTimeScaleNormal();
        }

        // Sets the game to normal speed.
        public void SetToNormalSpeed()
        {
            ResetGameTimeScale();
        }

        // Returns 'true' if the game is at a fast speed.
        public bool IsFastSpeed()
        {
            return GetGameTimeScale() == FAST_GAME_TIME_SCALE;
        }

        // Sets the game to fast speed.
        public void SetToFastSpeed()
        {
            SetGameTimeScale(FAST_GAME_TIME_SCALE);
        }

        // Toggles fast speed.
        public void ToggleFastSpeed()
        {
            // If the game time scale is normal, set to fast.
            // If the game time scale is fast,set it to normal.
            if(IsGameTimeScaleNormal())
            {
                SetToFastSpeed();
            }
            else
            {
                SetToNormalSpeed();
            }
        }

        // Returns 'true' if the game is at a slow speed.
        public bool IsSlowSpeed()
        {
            return GetGameTimeScale() == SLOW_GAME_TIME_SCALE;
        }

        // Sets the game to slow speed.
        public void SetToSlowSpeed()
        {
            SetGameTimeScale(SLOW_GAME_TIME_SCALE);
        }

        // Toggles slow speed.
        public void ToggleSlowSpeed()
        {
            // If the game time scale is normal, set to slow.
            // If the game time scale is fast,set it to normal.
            if (IsGameTimeScaleNormal())
            {
                SetToSlowSpeed();
            }
            else
            {
                SetToNormalSpeed();
            }
        }

        // UNIT OPERATIONS
        // Generates the conversion question for the player.
        public string GenerateConversionQuestion(Meteor meteor)
        {
            // The result.
            string result = string.Empty;

            // If 'true', fractions can be tried.
            bool tryFractions = UnitsInfo.IsMetricUnits(meteor.conversion.group) && 
                meteor.conversion.inputValue < 1.0F && meteor.conversion.inputValue >= 0.0F;

            // If fractions can be used, try to generate a fraction.
            if (useFractions && tryFractions)
            {
                // Generates a random value.
                float randValue = Random.Range(0.0F, 1.0F);

                // Fraction display change.
                if(randValue <= FRACTION_DISPLAY_CHANCE)
                {
                    // Variables to be used to set up the string.
                    float inputValue = meteor.conversion.inputValue;
                    string inputValueString = inputValue.ToString();
                    int decimalPlaces = 0;

                    // If there is a decimal place, use it to get the amount of decimal places.
                    if(inputValueString.Contains("."))
                    {
                        // Calculates the number of decimal places to know what to display.
                        decimalPlaces = inputValueString.Length - (inputValueString.IndexOf(".") + 1);
                    }

                    // There are decimal places, generate the result string.
                    if(decimalPlaces > 0)
                    {
                        float mult = Mathf.Pow(10, decimalPlaces);
                        result = (inputValue * mult).ToString() + "/" + mult.ToString() + " " +
                            meteor.conversion.GetInputSymbol() + " = ?";
                    }
                }

            }

            // Result wasn't set, so use the default format.
            if(result == "")
                result = meteor.conversion.inputValue.ToString() + " " + meteor.conversion.GetInputSymbol() + " = ?";

            return result;
        }

        // Calculates the points to be given for destroying the provided meteor.
        public float CalculatePoints(Meteor meteor)
        {
            // The points to be returned.
            float points = 0;
            
            // Base amount, combo bonus, and difficulty bonus.
            points += 15;
            points += 10 * combo;
            points += 5 * difficulty;

            // Returns the points.
            return points;
        }
        
        // Returns 'true' if the points goal has been reached.
        public bool IsPointsGoalReached(float points)
        {
            return points >= pointsGoal;
        }

        // Gets the progress of the player's points towards the goal.
        public float GetPlayerPointsProgress()
        {
            // Calculates the percent and returns it.
            float percent = player.GetPoints() / pointsGoal;
            return percent;
        }

        // Gets the player's points.
        public float GetPlayerPoints()
        {
            return player.GetPoints();
        }

        // Sets the player's points.
        public void SetPlayerPoints(float points)
        {
            player.SetPoints(points);
            player.OnPointsChanged();
        }

        // Called when the player's points have changed.
        public void OnPlayerPointsChanged()
        {
            // Updates the points bar.
            stageUI.UpdatePointsText();
            stageUI.UpdatePointsBar();

            // If the points goal has been reached, trigger the stage win.
            if (IsPointsGoalReached(player.GetPoints()))
            {
                OnStageWon();
            }
            else // Change phase.
            {
                SetPhaseByPlayerPointsProgress();
            }
        }

        // Calculates the final stage score and returns it.
        public float CalculateStageFinalScore()
        {
            // Base score.
            float score = player.GetPoints();

            // Difficulty bonus.
            score += 50.0F * difficulty;

            // Highest combo bonus.
            score += 100.0F * highestCombo;

            // Returns the score.
            return score;
        }

        // Calculates and set the stage final score.
        public void CalculateAndSetStageFinalScore()
        {
            // Sets the final score.
            stageFinalScore = CalculateStageFinalScore();

            // The score can't be negative.
            if (stageFinalScore < 0)
                stageFinalScore = 0;
        }

        // COMBO
        // Increaes the combo.
        public void IncreaseCombo()
        {
            // Increase the combo and reset the timer.
            combo++;
            comboTimer = COMBO_TIMER_MAX;

            // If this is the new highest combo, set it.
            if (combo > highestCombo)
                highestCombo = combo;
        }

        // Displays the combo.
        public void DisplayCombo(Vector3 position)
        {
            comboDisplay.PlayComboAnimationAtPosition(position);
        }

        // Resets the combo.
        public void ResetCombo(bool resetHighestCombo)
        {
            combo = 0;
            comboTimer = 0;

            // If the highest combo should be reset, reset it.
            if (resetHighestCombo)
                highestCombo = 0;
        }

        // ENDING
        // Called when the stage has ended.
        public void OnStageEnd()
        {
            runningGame = false;
            SetToNormalSpeed();
            PauseGame();
            FindAndKillAllMeteors();
        }

        // Called when the game has been won.
        public void OnStageWon()
        {
            // On stage end.
            OnStageEnd();

            // Calculate the final score.
            CalculateAndSetStageFinalScore();

            // Stage won.
            stageUI.OnStageWon();
        }

        // Called when the game has been lost.
        public void OnStageLost()
        {
            // On stage end.
            OnStageEnd();

            // Set time and score to 0.
            stageTime = 0;
            stageFinalScore = 0;

            // Add to the losses count, and adjusts the difficulty.
            losses++;
            AdjustDifficultyByLosses();

            // Stage lost.
            stageUI.OnStageLost();
        }

        // Called to restart the stage.
        public void ResetStage()
        {
            // Reset the player's points, kill all the meteors, and reset the game progress.
            player.SetPoints(0);
            KillAllMeteorsInList();
            SetPhaseByPlayerPointsProgress();

            // Resets the barrier and surface.
            foreach(Barrier barrier in stageBarriers)
            {
                // Restore the barrier.
                if (barrier != null)
                    barrier.RestoreBarrier();
            }

            // Restores the surface to full health.
            stageSurface.RestoreSurface();

            // Resets the time, unpauses the game, and starts running the game again.
            stageTime = 0;
            stageFinalScore = 0;

            // The difficulty is only dynamically adjusted if the player goes back to the world scene.

            // Restarts the stage.
            stageUI.OnStageReset();

            // Unpause game and start running.
            UnpauseGame();
            runningGame = true;
        }

        // Generates the stage data.
        public StageData GenerateStageData()
        {
            // The stage data.
            StageData data = new StageData();

            // Set values.
            data.stageName = stageName;
            data.stageTime = stageTime;
            data.stageScore = stageFinalScore;
            data.highestCombo = highestCombo;
            data.cleared = cleared;

            // Return the values.
            return data;
        }

        // Goes to the world.
        public void ToWorld()
        {
            OnGameEnd();

            // Saves the stage info and goes into the world.
            GameplayInfo.Instance.SaveStageInfo(this);
            ToWorldScene();
        }

        // Goes to the world scene.
        public void ToWorldScene()
        {
            SceneManager.LoadScene(worldScene);
        }

        // Called to run the game mechanics.
        public void RunGame()
        {
            // Reduce the spawn timer.
            meteorSpawnTimer -= Time.deltaTime;

            // Cap timer.
            if(meteorSpawnTimer <= 0)
                meteorSpawnTimer = 0;


            // Spawn a meteor.
            if(meteorSpawnTimer <= 0)
            {
                SpawnMeteor();

                // Sets the timer to the spawn rate.
                meteorSpawnTimer = GetModifiedMeteorSpawnRate();
            }

            // If there is no meteor being target.
            if(meteorTarget.meteor == null)
            {
                // Gets the closest meteor, and move towards it.
                meteorTarget.meteor = GetClosestMeteor();
                meteorTarget.trackExactPos = false;
            }

            // TODO: check points and damage for a game win?
        }
       
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the game is running.
            if(runningGame && !IsGamePaused())
            {
                // Add to the stage timer and updates the time text.
                // TODO: maybe don't update every frame?
                stageTime += Time.unscaledDeltaTime;
                stageUI.UpdateTimeText();

                // Run the game.
                RunGame();

                // If the combo timer is greater than 0, and a meteor is targeted.
                if(comboTimer > 0.0F && meteorTarget.meteor != null)
                {
                    // Reduce the timer.
                    comboTimer -= Time.deltaTime;

                    // Reset the combo if the timer has run out.
                    if(comboTimer <= 0.0F)
                    {
                        ResetCombo(false);
                    }
                }
            }
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