using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.Port;

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

        // The minimum difficulty.
        public const int DIFFICULTY_MIN = 1;

        // The maximum difficulty.
        public const int DIFFICULTY_MAX = 9;

        // The player for the stage.
        public PlayerStage player;

        // The target for the meteor.
        public MeteorTarget meteorTarget;

        // The stage surface.
        public StageSurface stageSurface;

        // The score that must be met to win the game.
        public float POINTS_GOAL = 100.0F;

        [Header("Conversions")]
        // The units used for the stage.
        public List<UnitsInfo.unitGroups> stageUnitGroups = new List<UnitsInfo.unitGroups>();

        // The conversions for the stage.
        public List<UnitsInfo.UnitsConversion> conversions;

        // The minimum units input value.
        public const float UNITS_INPUT_VALUE_MIN = 0.01F;

        // The maximum units input value.
        public const float UNITS_INPUT_VALUE_MAX = 1000.0F;

        [Header("Meteors")]
        // The meteor spawn rate.
        public float meteorSpawnRate = 1.0F;

        // The timer used for spawning meteors.
        private float meteorSpawnTimer = 0.0F;

        // The meteor fall speed factor.
        public float meteorSpeedMax = 1.0F;

        // The meteor prefabs.
        public List<Meteor> meteorPrefabs = new List<Meteor>();

        // TODO: The meteor pool. May not need this.
        // private List<Meteor> meteorPool = new List<Meteor>();

        // The active meteors.
        private List<Meteor> meteorsActive = new List<Meteor>();

        // The total number of meteors that can be active at once.
        private int ACTIVE_METEORS_COUNT_MAX = 12;

        // Gets set to 'true' when the game is running.
        private bool runningGame = false;

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

            // Sets the difficulty.
            SetDifficulty(difficulty);
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
                    conversions.AddRange(UnitsInfo.Instance.GetConversionList(group));
                    usedGroups.Add(group);
                }
            }

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
        public void SetDifficulty(int difficultyLevel)
        {
            // Sets the difficulty.
            difficulty = Mathf.Clamp(difficultyLevel, DIFFICULTY_MIN, DIFFICULTY_MAX);

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

            // Maximum meteor count met.
            if(meteorsActive.Count >= ACTIVE_METEORS_COUNT_MAX)
            {
                // Debug.LogWarning("Maximum meteor count met.");
                return null;
            }

            // Generates a meteor.
            int randomIndex = Random.Range(0, meteorPrefabs.Count);
            Meteor meteor = Instantiate(meteorPrefabs[randomIndex]);

            // Generates a random conversion for the meteor.
            meteor.conversion = GenerateRandomConversionFromList();
            SetRandomConversionInputValue(meteor.conversion);

            // Set the parent.
            if(stage.meteorParent != null)
                meteor.transform.parent = stage.meteorParent.transform;

            // Add to the list.
            meteorsActive.Add(meteor);

            // Generate the spawn point.
            meteor.spawnPoint = stage.GenerateMeteorSpawnPoint();

            // Called when the meteor has spawned.
            meteor.OnSpawn();

            // Return the meteor.
            return meteor;
        }

        // Removes the meteor from the active list if it's destroyed.
        public void OnMeteorDestroyed(Meteor meteor)
        {
            if(meteorsActive.Contains(meteor))
                meteorsActive.Remove(meteor);
        }

        // Gets the closest meteor from the active list.
        public Meteor GetClosestMeteor()
        {
            // The meteor and the distance.
            Meteor meteor = null;
            float meteorDist = 0;

            // Goes through all meteors from end to start.
            for (int i = meteorsActive.Count - 1; i >= 0; i--)
            {
                // If the index is null, remove it.
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
                        meteorDist = Vector3.Distance(meteor.transform.position, stageSurface.gameObject.transform.position);
                    }
                    else // Compare distances.
                    {
                        // Grabs the two meteors.
                        Meteor m1 = meteor;
                        Meteor m2 = meteorsActive[i];

                        // Gets the distances.
                        float m1Dist = meteorDist;
                        float m2Dist = Vector3.Distance(m2.transform.position, stageSurface.gameObject.transform.position);

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
            // The meteors active.
            for (int i = meteorsActive.Count - 1; i >= 0; i--)
            {
                // If the index is null, remove it.
                if (meteorsActive[i] == null)
                {
                    meteorsActive.RemoveAt(i);
                }
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
                conversion = conversions[Random.Range(0, conversions.Count)];
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
        public void SetRandomConversionInputValue(UnitsInfo.UnitsConversion conversion)
        {
            // Generates the value.
            float value = Random.Range(UNITS_INPUT_VALUE_MIN, UNITS_INPUT_VALUE_MAX);

            // Round the value, and cap it at 3 decimal places.
            value *= 1000.0F;
            value = Mathf.Round(value);
            value /= 1000.0F;

            // Return the value.
            conversion.inputValue = value;
        }

        // OPERATIONS
        // Generate a question for the player to answer.
        public void GenerateQuestion()
        {
            // TODO: implement.
        }

        // Calculates the points to be given for destroying the provided meteor.
        public float CalculatePoints(Meteor meteor)
        {
            // TODO: implement
            return 10;
        }

        // Returns 'true' if the points goal has been reached.
        public bool IsPointsGoalReached(float points)
        {
            return points >= POINTS_GOAL;
        }


        // ENDING
        // Called when the game has been won.
        public void OnStageWon()
        {
            // Win Event
        }

        // Called when the game has been lost.
        public void OnStageLost()
        {
            // Lose Event
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

        // Called to run the game.
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
                meteorSpawnTimer = meteorSpawnRate;
            }

            // If there is no meteor being target.
            if(meteorTarget.meteor == null)
            {
                // Gets the closest meteor.
                meteorTarget.meteor = GetClosestMeteor();
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
                RunGame();
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