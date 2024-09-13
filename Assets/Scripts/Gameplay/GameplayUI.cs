using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_MST
{
    // The game UI.
    public class GameplayUI : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        [Header("Menu")]
        // The text box panel.
        public Image menuPanel;

        // The settings UI.
        // TODO: add quit button.
        public GameSettingsUI settingsUI;

        [Header("Tutorial")]

        // The tutorial UI.
        public TutorialUI tutorialUI;

        // The text box panel.
        public Image tutorialPanel;

        // The tutorial text box.
        public TutorialTextBox tutorialTextBox;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the tutorial UI is not set, set it.
            if (tutorialUI == null)
                tutorialUI = TutorialUI.Instance;
        }

        // TUTORIAL //
        // Start tutorial
        public void StartTutorial(List<Page> pages)
        {
            // Sets the pages and opens the text box.
            tutorialTextBox.textBox.pages = pages;
            tutorialTextBox.textBox.CurrentPageIndex = 0;
            tutorialTextBox.textBox.Open();
        }

        // On Tutorial Start
        public virtual void OnTutorialStart()
        {
            // Turn on the tutorial panel.
            if(tutorialPanel != null)
                tutorialPanel.gameObject.SetActive(true);
        }

        // On Tutorial End
        public virtual void OnTutorialEnd()
        {
            // Turns off the tutorial panel.
            if(tutorialPanel != null)
                tutorialPanel.gameObject.SetActive(false);
        }

        // Checks if the tutorial text box is open.
        public bool IsTutorialTextBoxOpen()
        {
            return tutorialTextBox.textBox.IsVisible();
        }

        // Returns 'true' if the tutorial can be started.
        public bool IsTutorialAvailable()
        {
            return !IsTutorialTextBoxOpen();
        }

        // Adds the tutorial text box open/close callbacks.
        public void AddTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialTextBox.textBox.OnTextBoxOpenedAddCallback(manager.OnTutorialStart);
            tutorialTextBox.textBox.OnTextBoxClosedAddCallback(manager.OnTutorialEnd);
        }

        // Removes the tutorial text box open/close callbacks.
        public void RemoveTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialTextBox.textBox.OnTextBoxOpenedRemoveCallback(manager.OnTutorialStart);
            tutorialTextBox.textBox.OnTextBoxClosedRemoveCallback(manager.OnTutorialEnd);
        }

        // WINDOWS //
        // Opens the settings.
        public void OpenSettings()
        {
            settingsUI.gameObject.SetActive(true);
            OnWindowOpened(settingsUI.gameObject);
        }

        // Closes the settings.
        public void CloseSettings()
        {
            // Close the settings window.
            settingsUI.gameObject.SetActive(false);
            OnWindowClosed();

            // Close all the windows.
            // CloseAllWindows();
        }

        // Toggles the settings on and off.
        public void ToggleSettings()
        {
            // If the settings is active, close it.
            if(settingsUI.gameObject.activeSelf) 
            {
                CloseSettings();
            }
            else // Open the settings.
            {
                OpenSettings();
            }
        }

        // Closes all the windows.
        public virtual void CloseAllWindows()
        {
            // Settings
            settingsUI.gameObject.SetActive(false);
            
            // On Window Closed
            OnWindowClosed();
        }

        // Called when a window is opened.
        public virtual void OnWindowOpened(GameObject window)
        {
            gameManager.PauseGame();

            // Turns off the settings window if it wasn't the one that got turned on.
            if (window != settingsUI.gameObject)
                settingsUI.gameObject.SetActive(false);

            // Enables the menu panel to block the UI under it.
            if (menuPanel != null)
                menuPanel.gameObject.SetActive(true);


            // If the tutorial text box is open.
            if(IsTutorialTextBoxOpen() && tutorialPanel.gameObject.activeSelf)
            {
                // Turns off the tutorial panel so that they aren't overlayed.
                tutorialPanel.gameObject.SetActive(false);
            }
        }

        // Called when a window is closed.
        public virtual void OnWindowClosed()
        {
            // Checks for the tutorial text box.
            if(tutorialTextBox != null)
            {
                // Unpause the game only if the tutorial textbox is closed.
                if(!tutorialTextBox.textBox.IsVisible())
                    gameManager.UnpauseGame();

            }
            else // Regular unpause.
            {
                gameManager.UnpauseGame();
            }

            // Disables the tutorial panel.
            if(menuPanel != null)
                menuPanel.gameObject.SetActive(false);

            // If the tutorial text box is open.
            if (IsTutorialTextBoxOpen())
            {
                // Turns on the tutorial panel since the menu panel isn't showing now.
                tutorialPanel.gameObject.SetActive(true);
            }
        }

        // Checks if a window is open.
        public virtual bool IsWindowOpen()
        {
            // Only checks the settings window here.
            bool open = settingsUI.gameObject.activeSelf;

            return open;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}