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

        [Header("Windows/Menus")]
        // The window panel.
        public Image windowPanel;

        // The settings UI.
        // TODO: add quit button.
        public GameSettingsUI gameSettingsUI;

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
            // If the game manager isn't set, try to find it.
            if(gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>();

            // If the tutorial UI is not set, set it.
            if (tutorialUI == null)
                tutorialUI = TutorialUI.Instance;

            // Closes all windows by default.
            CloseAllWindows();
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
        // Checks if a window is open.
        public virtual bool IsWindowOpen()
        {
            // Only checks the settings window here.
            bool open = gameSettingsUI.gameObject.activeSelf;

            return open;
        }

        // Closes all the windows.
        public virtual void CloseAllWindows()
        {
            // Settings
            gameSettingsUI.gameObject.SetActive(false);
            
            // On Window Closed
            OnWindowClosed();
        }

        // Opens the provided window.
        public virtual void OpenWindow(GameObject window)
        {
            CloseAllWindows();
            window.gameObject.SetActive(true);
            OnWindowOpened(window);
        }

        // Called when a window is opened.
        public virtual void OnWindowOpened(GameObject window)
        {
            gameManager.PauseGame();

            // Enables the menu panel to block the UI under it.
            if (windowPanel != null)
                windowPanel.gameObject.SetActive(true);

            // If the tutorial text box is open.
            if(IsTutorialTextBoxOpen() && tutorialPanel != null)
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
            if(windowPanel != null)
                windowPanel.gameObject.SetActive(false);

            // If the tutorial text box is open.
            if (IsTutorialTextBoxOpen())
            {
                // Turns on the tutorial panel since the menu panel isn't showing now.
                if(tutorialPanel != null)
                    tutorialPanel.gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}