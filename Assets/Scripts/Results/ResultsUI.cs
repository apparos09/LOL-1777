using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_MST
{
    // The results UI.
    public class ResultsUI : MonoBehaviour
    {
        public ResultsManager manager;

        [Header("Text")]

        // The game time text.
        public TMP_Text gameTimeText;

        // The score text.
        public TMP_Text gameScoreText;

        // TODO: add answer speed?

        [Header("Buttons")]

        // Button for going to the title screen.
        public Button titleButton;

        // Button for completing the game.
        public Button finishButton;

        // Start is called before the first frame update
        void Start()
        {
            // Manager
            if (manager == null)
                manager = ResultsManager.Instance;

            // If the platform is set to webGL, disable the quit button.
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                titleButton.interactable = false; // Disable
                
            }

            // If the LOLSDK has been initialized.
            if (GameSettings.InitializedLOLSDK)
            {
                // Turn off the title button.
                titleButton.gameObject.SetActive(false);
            }
        }

        // Applies the results data.
        public void ApplyResultsData(ResultsData data)
        {
            // Time
            gameTimeText.text = StringFormatter.FormatTime(data.gameTime, false, true, false);

            // Score
            gameScoreText.text = data.gameScore.ToString();
        }

        // Goes to the title scene.
        public void ToTitleScene()
        {
            manager.ToTitleScene();
        }
        
        // Complete the game.
        public void CompleteGame()
        {
            manager.CompleteGame();
        }
    }
}