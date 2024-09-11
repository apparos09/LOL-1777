using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // This script is used for game info that's shared between all scenes.
    public class GameplayInfo : MonoBehaviour
    {
        // The singleton instance.
        private static GameplayInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game time.
        public float gameTime = 0.0F;

        // The game score.
        public int gameScore = 0;

        //// The number of wrong answers from the player.
        //[Tooltip("The total number of wrong answers from P1.")]
        //public int wrongAnswers = 0;

        //// The number of losses from P1.
        //[Tooltip("The total number of losses from P1.")]
        //public int p1Losses = 0;

        //// The number of recent losses from P1. When P1 wins a match, this is reset.
        //[Tooltip("The recent number of losses from P1. When P1 wins a match, this is reset.")]
        //public int p1RecentLosses = 0;

        //// The powers p1 has.
        //public List<Power.powerType> p1PowerList = new List<Power.powerType>();

        //// WORLD
        //[Header("World Info")]

        //// Gets set to 'true' when world data is saved.
        //// Maybe reset this if you ever want to reload data.
        //[Tooltip("Gets set to 'true' when world data has been saved.")]
        //public bool worldDataSaved = false;

        //// The index of the current world area.
        //public int currAreaIndex = 0;

        //// A list that stores what challengers have and have not been defeated.
        //public List<bool> challengersDefeated = new List<bool>();


        //// MATCH
        //[Header("Match Info")]

        //// Gets set to 'true' when match data has been saved.
        //[Tooltip("Gets set to 'true' when match data has been saved.")]
        //public bool matchDataSaved = false;

        //// The number of the match. This is calculated in the world area, and is copied to the match area.
        //// Do not overwrite this with info from the match manager.
        //[Tooltip("The match number, which is set by the world manager via a function.")]
        //public int matchNumber = 0;

        //// The puzzle type.
        //public Puzzle.puzzleType puzzle = Puzzle.puzzleType.unknown;

        //// The number of the winner (1 = player 1, 2 = player 2/computer, else unknown)
        //[Tooltip("The player number of the winner. 1 is P1, 2 is P2/COM, else is unknown.")]
        //public int pWinner = -1;

        //[Header("Match Info/Exponents")]

        //// The exponent rates.
        //public float exponentRate = 1.0F;

        //// Rate for multiplicaton (same bases)/product rule exponents.
        //public float productRate = 1.0F;

        //// Rate for exponent by exponent/power of a power rule exponents.
        //public float powerOfAPowerRate = 1.0F;

        //// Rate for multplication (different bases)/power of a product rule exponents.
        //public float powerOfAProductRate = 1.0F;

        //// Rate for zero exponents.
        //public float zeroRate = 1.0F;

        //// Rate for negative exponents.
        //public float negativeRate = 1.0F;

        //// NOTE: you may not even change the defaults.
        //[Header("Match Info/Puzzle Settings")]
        //// The point goal for the game.
        //public int pointGoal = 600; // 999

        //// If 'true', the point goal is used.
        //public bool usePointGoal = true;

        //// The lowest value can equation will use.
        //public int equationLowestValue = 0;

        //// The highest value an equation will use.
        //public int equationHighestValue = 9;

        //// Minimum equation term.
        //[Tooltip("The minimum number of equation terms.")]
        //public int equationTermsMin = 1;

        //// Maximum equation term.
        //[Tooltip("The maximum number of equation terms.")]
        //public int equationTermsMax = 1;

        //// The minimum number of terms for the base exponent rule.
        //[Tooltip("The minimum number of terms for the base exponent rule (combined rules only).")]
        //public int exponentTermsMin = 1;

        //// The maximum number of terms for the base exponent rule.
        //[Tooltip("The maximum number of terms for the base exponent rule (combined rules only).")]
        //public int exponentTermsMax = 3;

        //// The minimum number of missing values.
        //[Tooltip("The minimum number of missing values.")]
        //public int missingValuesMin = 1;

        //// The maximum number of missing values.
        //[Tooltip("The maximum number of missing values.")]
        //public int missingValuesMax = 1;


        //[Header("Match Info/Players")]

        //// Player 1's power type.
        //public Power.powerType p1Power = Power.powerType.none;

        //// Player 2's power type.
        //public Power.powerType p2Power = Power.powerType.none;

        //// The index of the challenger in the world list.
        //public int challengerIndex = -1;

        //// TODO: add challenger icon?

        //// The difficulty of the challenger.
        //// NOTE: if the challenger difficulty is 0 or less, the equation details WON'T be overwritten.
        //public int challengerDifficulty = 0;

        //// If 'true', dynamic difficulty will be used.
        //public const bool USE_DYNAMIC_DIFFICULTY = true;

        //// The multiple for dynamic difficulty changes.
        //public const int DIFFICULTY_LOWER_MULT = 2;

        //// Checks if the challenger has been defeated.
        //public bool challengerDefeated = false;

        //// Gets set to 'true' if it's the final challenger.
        //public bool isFinalChallenger = false;

        //[Header("Match Info/Other")]

        //// The background for the match.
        //public int matchBackgroundIndex = -1;

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

        //// Saves general game info.
        //protected void SaveGameInfo(GameplayManager manager)
        //{
        //    // Time
        //    gameTime = manager.gameTime;

        //    // Score
        //    gameScore = manager.gameScore;


        //    // Checks what kind of data was just saved.
        //    if(manager is WorldManager) // World
        //    {
        //        worldDataSaved = true;
        //    }
        //    else if(manager is MatchManager) // Match
        //    {
        //        matchDataSaved = true;
        //    }
        //}

        //// Loads general game info.
        //protected void LoadGameInfo(GameplayManager manager)
        //{
        //    // Time
        //    manager.gameTime = gameTime;

        //    // Score
        //    manager.gameScore = gameScore;
        //}

        //// WORLD //
        //// Saves world info from the world manager.
        //public void SaveWorldInfo(WorldManager manager)
        //{
        //    SaveGameInfo(manager);

        //    // Save the player powers by copying over the list.
        //    p1PowerList = new List<Power.powerType>(manager.playerWorld.powerList);

        //    // Save the index.
        //    currAreaIndex = manager.currAreaIndex;

        //    // TODO: not needed?
        //    // // Save the puzzle type
        //    // puzzle = manager.worldUI.challengeUI.challenger.puzzle;

        //    // Clears out the list.
        //    challengersDefeated.Clear();

        //    // Goes through all challengers.
        //    for(int i = 0; i < manager.challengers.Count; i++)
        //    {
        //        // Saves the defeated challengers.
        //        challengersDefeated.Add(manager.challengers[i].IsChallengerDefeated());
        //    }

        //    // Reset the final challenger variable.
        //    isFinalChallenger = false;

        //    // Saves the match number.
        //    matchNumber = manager.GetMatchNumber();
        //}

        //// Saves world info from the match manager.
        //public void SaveWorldInfo(MatchManager manager)
        //{
        //    SaveGameInfo(manager);

        //    // Updates the match winner if it hasn't been updated already.
        //    pWinner = manager.matchWinner;

        //    // If the challenger has not been defeated yet, check if the challenger has been defeated now.
        //    if (!challengerDefeated && pWinner > 0 && pWinner <= 2)
        //        challengerDefeated = (pWinner == 1);

        //    // Updates the challenger that was defeated.
        //    if (challengerIndex != 1)
        //        challengersDefeated[challengerIndex] = challengerDefeated;
        //}

        //// Loads world info into manager.
        //public void LoadWorldInfo(WorldManager manager)
        //{
        //    LoadGameInfo(manager);

        //    // Moved set area to the end.

        //    // Update the current challenger defeat status if the index is valid.
        //    if(challengerIndex > 0 && challengerIndex < challengersDefeated.Count)
        //        challengersDefeated[challengerIndex] = challengerDefeated;

        //    // Goes through all challengers and sets if they're defeated or not.
        //    for (int i = 0; i < manager.challengers.Count && i < challengersDefeated.Count; i++)
        //    {
        //        // Set if the challenger's been defeated.
        //        manager.challengers[i].SetChallengerDefeated(challengersDefeated[i]);
        //    }

        //    // Set player's powers.
        //    manager.playerWorld.powerList = new List<Power.powerType>(p1PowerList); // Power List
        //    manager.playerWorld.SetPower(p1Power); // Set Power


        //    // Unlocking a Power
        //    // If player 1 (user) won the most recent match.
        //    if(pWinner == 1)
        //    {
        //        // If player 2 had a power...
        //        if(p2Power != Power.powerType.none)
        //        {
        //            // Checks if the player has P2's power or not.
        //            if (!manager.playerWorld.HasPower(p2Power))
        //            {
        //                // Give power.
        //                manager.playerWorld.GivePower(p2Power, false);

        //                // Play the power unlock animation.
        //                if (manager.worldAnimation != null)
        //                {
        //                    manager.worldAnimation.PlayPowerUnlockAnimation();
        //                }
        //            }
        //        }
        //    }

        //    // Overwite the power list with the new updates.
        //    // TODO: does this need to happen every time?
        //    p1PowerList = new List<Power.powerType>(manager.playerWorld.powerList);


        //    // For some reason this is causing issues with defeated parameter.
        //    // Updates the area events.
        //    // manager.UpdateAreaEvents();

        //    // Set the area.
        //    manager.SetArea(currAreaIndex, true);
        //}



        //// MATCH //
        //// Save the match info from the match manager.
        //public void SaveMatchInfo(MatchManager manager)
        //{
        //    // Saves the game info.
        //    SaveGameInfo(manager);

        //    // Saves the match winner.
        //    pWinner = manager.matchWinner;

        //    // Checks the winner.
        //    switch(pWinner)
        //    {
        //        case 1: // P1
        //            p1RecentLosses = 0;

        //            break;

        //        case 2: // P2
        //            // Increases the number of losses if P1 lost the match.
        //            p1Losses++;
        //            p1RecentLosses++;
                    
        //            break;
        //    }   

        //    // Add the wrong answers from the match.
        //    wrongAnswers += manager.p1WrongAnswers;
        //}

        //// Stores the match info from the world manager to be used in the match info.
        //public void SaveMatchInfo(WorldManager manager)
        //{
        //    // CHALLENGER
        //    // Grabs the challenger.
        //    ChallengerWorld challenger = manager.worldUI.challengeUI.challenger;

        //    // Checks if the challenger is the final challenger.
        //    if (manager.finalChallenger != null)
        //    {
        //        // If this is the final challenger, set this to true.
        //        isFinalChallenger = manager.IsFinalChallenger(challenger);
        //    }
        //    else
        //    {
        //        isFinalChallenger = false;
        //    }


        //    // Save the game time.
        //    gameTime = manager.gameTime;

        //    // Save the area index as the match background index.
        //    matchBackgroundIndex = manager.currAreaIndex;

        //    // PUZZLE/CHALLENGE INFO
        //    // Sets the puzzle type.
        //    puzzle = challenger.puzzle;

        //    // Exponents
        //    // Base, Mult Same, Expo By Expo
        //    exponentRate = challenger.exponentRate;
        //    productRate = challenger.productRate;
        //    powerOfAPowerRate = challenger.powerOfAPowerRate;

        //    // Mult Diff, Zero, Negative
        //    powerOfAProductRate = challenger.powerOfAProductRate;
        //    zeroRate = challenger.zeroRate;
        //    negativeRate = challenger.negativeRate;


        //    // CHALLENGER
        //    // The challenger's index in the world list.
        //    challengerIndex = manager.GetChallengerIndex(challenger);

        //    // Sets difficulty.
        //    challengerDifficulty = challenger.difficulty;

        //    // Difficulty - Dynamic Changes
        //    {
        //        // Checks if dynamic difficulty should be used.
        //        bool dynamicDiff = USE_DYNAMIC_DIFFICULTY;

        //        // Use dynamic difficulty.
        //        if(dynamicDiff)
        //        {
        //            // Determines how much lower the difficlty should go.
        //            int lowers = Mathf.FloorToInt((float)p1RecentLosses / DIFFICULTY_LOWER_MULT);

        //            // Sets to aboslute value.
        //            lowers = Mathf.Abs(lowers);

        //            // Reduces difficluty.
        //            challengerDifficulty -= lowers;

        //            // If the difficulty is less than 1, set it to 1.
        //            if (challengerDifficulty < 1)
        //                challengerDifficulty = 1;
        //        }
                
        //    }

        //    // Saves if the challenger was defeated.
        //    challengerDefeated = challenger.IsChallengerDefeated();

        //    // Match settings from challenger.
        //    // Equation Terms
        //    equationTermsMin = challenger.equationTermsMin;
        //    equationTermsMax = challenger.equationTermsMax;

        //    // Exponent Terms
        //    exponentTermsMin = challenger.exponentTermsMin;
        //    exponentTermsMax = challenger.exponentTermsMax;

        //    // Missing Values
        //    missingValuesMin = challenger.missingValuesMin;
        //    missingValuesMax = challenger.missingValuesMax;


        //    // PLAYERS
        //    // Sets the powers.
        //    p1Power = manager.playerWorld.power;
        //    p2Power = challenger.power;
        //}

        //// Loads match info into the manager.
        //public void LoadMatchInfo(MatchManager manager)
        //{
        //    LoadGameInfo(manager);

        //    // Set the match number.
        //    manager.matchNumber = matchNumber;

        //    // Sets the background.
        //    manager.SetBackground(matchBackgroundIndex);

        //    // PUZZLE //
        //    // Set the puzzle type.
        //    manager.p1Puzzle.puzzle = puzzle;
        //    manager.p2Puzzle.puzzle = puzzle;

        //    // Puzzle Mechanics
        //    // Grabs the instance.
        //    PuzzleInfo puzzlePrefabs = PuzzleInfo.Instance;

        //    // The mechanics for P1 and P2.
        //    PuzzleMechanic p1Mech, p2Mech;

        //    // TODO: add other puzzle types and prefabs.
        //    // Checks the puzzle type.
        //    switch (puzzle)
        //    {
        //        // Generates a keypad by default.
        //        default:
        //        case Puzzle.puzzleType.keypad: // Keypad
        //            p1Mech = Instantiate(puzzlePrefabs.keypad);
        //            p2Mech = Instantiate(puzzlePrefabs.keypad);
        //            break;

        //        case Puzzle.puzzleType.bubbles: // Bubbles
        //            p1Mech = Instantiate(puzzlePrefabs.bubble);
        //            p2Mech = Instantiate(puzzlePrefabs.bubble);
        //            break;

        //        case Puzzle.puzzleType.sliding: // Sliding
        //            p1Mech = Instantiate(puzzlePrefabs.sliding);
        //            p2Mech = Instantiate(puzzlePrefabs.sliding);
        //            break;

        //        case Puzzle.puzzleType.pinball: // Pinball
        //            p1Mech = Instantiate(puzzlePrefabs.pinball);
        //            p2Mech = Instantiate(puzzlePrefabs.pinball);
        //            break;
        //    }

        //    // Change the names.
        //    p1Mech.name += " (P1)";
        //    p2Mech.name += " (P2)";

        //    // Set P1 Puzzle Mechanic Parent and Position
        //    p1Mech.transform.parent = manager.p1Puzzle.transform; // Parent

        //    if (manager.p1Puzzle.puzzleMechanicPos != null) // Position
        //        p1Mech.transform.position = manager.p1Puzzle.puzzleMechanicPos.transform.position;


        //    // Set P2 Puzzle Mechanic Parent and Position
        //    p2Mech.transform.parent = manager.p2Puzzle.transform; // Parent

        //    if (manager.p2Puzzle.puzzleMechanicPos != null) // Position
        //        p2Mech.transform.position = manager.p2Puzzle.puzzleMechanicPos.transform.position;

        //    // Set the managers.
        //    p1Mech.manager = manager;
        //    p2Mech.manager = manager;

        //    // Set the mechanics for the puzzles, and vice versa.
        //    // Mechanics
        //    manager.p1Puzzle.puzzleMechanic = p1Mech;
        //    manager.p2Puzzle.puzzleMechanic = p2Mech;

        //    // Puzzles
        //    p1Mech.puzzle = manager.p1Puzzle;
        //    p2Mech.puzzle = manager.p2Puzzle;


        //    // EXPONENTS //
        //    // Base
        //    manager.p1Puzzle.exponentRate = exponentRate;
        //    manager.p2Puzzle.exponentRate = exponentRate;

        //    // Mult Same
        //    manager.p1Puzzle.productRate = productRate;
        //    manager.p2Puzzle.productRate = productRate;

        //    // Expo By Expo
        //    manager.p1Puzzle.powerOfAPowerRate = powerOfAPowerRate;
        //    manager.p2Puzzle.powerOfAPowerRate = powerOfAPowerRate;

        //    // Mult Diff
        //    manager.p1Puzzle.powerOfAProductRate = powerOfAProductRate;
        //    manager.p2Puzzle.powerOfAProductRate = powerOfAProductRate;

        //    // Zero
        //    manager.p1Puzzle.zeroRate = zeroRate;
        //    manager.p2Puzzle.zeroRate = zeroRate;

        //    // Negative
        //    manager.p1Puzzle.negativeRate = negativeRate;
        //    manager.p2Puzzle.negativeRate = negativeRate;


        //    // MATCH SETTINGS //
        //    manager.pointGoal = pointGoal;
        //    manager.usePointGoal = usePointGoal;

        //    // Lowest equation values
        //    manager.p1Puzzle.equationLowestValue = equationLowestValue;
        //    manager.p2Puzzle.equationLowestValue = equationLowestValue;

        //    // Highest equation values
        //    manager.p1Puzzle.equationHighestValue = equationHighestValue;
        //    manager.p2Puzzle.equationHighestValue = equationHighestValue;

        //    // Equation terms (minimum)
        //    manager.p1Puzzle.equationTermsMin = equationTermsMin;
        //    manager.p2Puzzle.equationTermsMin = equationTermsMin;

        //    // Equation terms (maximum)
        //    manager.p1Puzzle.equationTermsMax = equationTermsMax;
        //    manager.p2Puzzle.equationTermsMax = equationTermsMax;

        //    // Base exponent terms (minimum)
        //    manager.p1Puzzle.exponentTermsMin = exponentTermsMin;
        //    manager.p2Puzzle.exponentTermsMin = exponentTermsMin;

        //    // Base exponent terms (maximum)
        //    manager.p1Puzzle.exponentTermsMax = exponentTermsMax;
        //    manager.p2Puzzle.exponentTermsMax = exponentTermsMax;

        //    // Missing values (minimum)
        //    manager.p1Puzzle.missingValuesMin = missingValuesMin;
        //    manager.p2Puzzle.missingValuesMin = missingValuesMin;

        //    // Missing values (maximum)
        //    manager.p1Puzzle.missingValuesMax = missingValuesMax;
        //    manager.p2Puzzle.missingValuesMax = missingValuesMax;


        //    // PLAYERS //

        //    // Powers
        
        //    // Goes through both players.
        //    for(int n = 1; n <= 2; n++)
        //    {
        //        // The player and the power.
        //        PlayerMatch player;
        //        Power.powerType power = Power.powerType.none;

        //        // Checks what player to use.
        //        switch(n)
        //        {
        //            case 1: // P1
        //                player = manager.p1;
        //                power = p1Power;
        //                break;

        //            case 2: // P2
        //                player = manager.p2;
        //                power = p2Power;
        //                break;

        //            default: // None
        //                continue;
        //        }

        //        // Sets the player's power.
        //        player.SetPower(power);
        //    }

        //    // Player 1 (Other)
        //    // ...

        //    // Player 2/Computer (Other)
        //    ComputerMatch cpu;

        //    // Gets the computer player.
        //    if (manager.p2.TryGetComponent<ComputerMatch>(out cpu))
        //    {
        //        // Sets difficulty.
        //        cpu.SetDifficulty(challengerDifficulty);
        //    }

        //    // Setting match BGM.
        //    if(manager.matchAudio != null)
        //    {
        //        // Gets the match audio.
        //        MatchAudio matchAudio = manager.matchAudio;

        //        // Checks if the player is facing the final challenger.
        //        if(isFinalChallenger) // Yes
        //        {
        //            // Normal BGM
        //            matchAudio.matchBgmNumber = 2;
        //        }
        //        else // No
        //        {
        //            // Boss BGM
        //            matchAudio.matchBgmNumber = 1;
        //        }
        //    }
        //}

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