using RM_MST;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_MST
{
    // The gameplay manager.
    public class GameplayManager : MonoBehaviour
    {
        // The game UI.
        public GameplayUI gameUI;

        // The timer for the game.
        public float gameTime = 0;

        // The game score
        public int gameScore = 0;

        // Pauses the timer if true.
        protected bool gamePaused = false;

        // The mouse touch object.
        public MouseTouchInput mouseTouch;

        // NOTE: GameInfo and Tutorial aren't listed here because they're singletons.
        // Having them be in the scene from the start caused issues, so I'm not going to have them.

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // For some reason, when coming back from the match scene this is listed as 'missing'.

            // Creates/gets the game info instance.
            GameplayInfo gameInfo = GameplayInfo.Instance;

            // // Creates/gets the tutorial instance if it will be used.
            // if(IsUsingTutorial())
            // {
            //     Tutorial tutorial = Tutorial.Instance;
            // }

            // Creates a tutorial instance.
            Tutorials tutorial = Tutorials.Instance;


            // If the gameUI is set, check for the tutorial text box.
            if (gameUI != null)
            {
                // If the tutorial text box is set...
                if (gameUI.tutorialTextBox != null)
                {
                    // Adds the callbakcs from the tutorial text box.
                    // I don't think I need to remove them.
                    gameUI.AddTutorialTextBoxCallbacks(this);
                }
            }
        }

        // Returns the provided time (in seconds), formatted.
        public static string GetTimeFormatted(float seconds, bool roundUp = true)
        {
            // Gets the time and rounds it up to the nearest whole number.
            float time = (roundUp) ? Mathf.Ceil(seconds) : seconds;

            // Formats the time.
            string formatted = StringFormatter.FormatTime(time, false, true, false);

            // Returns the formatted time.
            return formatted;
        }

        // Returns 'true' if the game is paused.
        public bool IsGamePaused()
        {
            return gamePaused;
        }

        // Sets if the game should be paused.
        public virtual void SetGamePaused(bool paused)
        {
            gamePaused = paused;
        }

        // Pauses the game.
        public virtual void PauseGame()
        {
            SetGamePaused(true);
        }

        // Unpauses the game.
        public virtual void UnpauseGame()
        {
            SetGamePaused(false);
        }

        // Toggles if the game is paused or not.
        public virtual void TogglePausedGame()
        {
            SetGamePaused(!gamePaused);
        }

        // TUTORIAL //

        // Checks if the game is using the tutorial.
        public bool IsUsingTutorial()
        {
            bool result = GameSettings.Instance.UseTutorial;
            return result;
        }

        // Set if the tutorial will be used.
        public void SetUsingTutorial(bool value)
        {
            GameSettings.Instance.UseTutorial = value;
        }

        // Returns 'true' if the tutorial is available to be activated.
        public bool IsTutorialAvailable()
        {
            return gameUI.IsTutorialAvailable();
        }

        // Checks if the text box is open.
        public bool IsTutorialTextBoxOpen()
        {
            return gameUI.IsTutorialTextBoxOpen();
        }

        // Starts a tutorial using the provided pages.
        public virtual void StartTutorial(List<Page> pages)
        {
            gameUI.StartTutorial(pages);
        }

        // Called when a tutorial is started.
        public virtual void OnTutorialStart()
        {
            gameUI.OnTutorialStart();
        }

        // Called when a tutorial is ended.
        public virtual void OnTutorialEnd()
        {
            gameUI.OnTutorialEnd();
        }

        // Called when the game is completed.
        public virtual void OnGameComplete()
        {
            ToResultsScene();
        }

        // SCENES //
        // Go to the title scene.
        public virtual void ToTitleScene()
        {
            // Destroys 'DontDestroyOnLoad' Objects
            // Game Info
            if (GameplayInfo.Instantiated)
                Destroy(GameplayInfo.Instance.gameObject);

            // Tutorial
            if (Tutorials.Instantiated)
                Destroy(Tutorials.Instance.gameObject);

            // TODO: add loading screen.
            SceneManager.LoadScene("TitleScene");
        }

        // Go to the resultsscene.
        public virtual void ToResultsScene()
        {
            // If the game info object exists, destroy it.
            if (GameplayInfo.Instantiated)
            {
                Destroy(GameplayInfo.Instance.gameObject);
            }

            // If the tutorial object exists, destroy it.
            if (Tutorials.Instantiated)
            {
                Destroy(Tutorials.Instance.gameObject);
            }


            // TODO: add loading screen.
            SceneManager.LoadScene("ResultsScene");
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // The game isn't paused.
            if (!gamePaused)
            {
                gameTime += Time.unscaledDeltaTime;
            }
        }
    }
}