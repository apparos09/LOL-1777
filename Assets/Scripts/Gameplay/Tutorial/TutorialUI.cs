using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_MST
{
    // The UI for the tutorial.
    public class TutorialUI : MonoBehaviour
    {
        // The singleton instance.
        private static TutorialUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game UI.
        public GameplayUI gameUI;

        // The tutorials object.
        public Tutorials tutorials;

        // The tutorial text box.
        public TutorialTextBox textBox;

        [Header("Diagram")]
        // The text box image object.
        public GameObject textBoxDiagram;

        // The text box image.
        public Image textBoxDiagramImage;


        [Header("Diagram/Images")]
        
        // TODO: take this out.

        // The alpha 0 sprite. Used to hide the diagram if there's no image.
        [Tooltip("Used to hide the text box diagram image.")]
        public Sprite alpha0Sprite;

        // Constructor
        private TutorialUI()
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
                textBox.textBox.OnTextBoxOpenedAddCallback(OnTextBoxOpened);
                textBox.textBox.OnTextBoxClosedAddCallback(OnTextBoxClosed);
                textBox.textBox.OnTextBoxFinishedAddCallback(OnTextBoxFinished);

                instanced = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the UI instance if it's not set.
            if (gameUI == null)
            {
                // Checks for the user interfaces to attach.
                if(WorldUI.Instantiated)
                {
                    gameUI = WorldUI.Instance;
                }
                else if(StageUI.Instantiated)
                {
                    gameUI = StageUI.Instance;
                }
                else
                {
                    Debug.LogWarning("Game UI could not be found.");
                }
                    
            }

            // Gets the tutorials object.
            if (tutorials == null)
                tutorials = Tutorials.Instance;

            // If the text box is open, close it.
            if(textBox.gameObject.activeSelf)
            {
                textBox.gameObject.SetActive(false);
            }
        }

        // Gets the instance.
        public static TutorialUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<TutorialUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Tutorial UI (singleton)");
                        instance = go.AddComponent<TutorialUI>();
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

        // Is the tutorial active?
        public bool IsTutorialRunning()
        {
            // If the textbox is isible, then the tutorial is active.
            return textBox.textBox.IsVisible();
        }

        // Starts a tutorial.
        public void StartTutorial()
        {
            textBox.textBox.SetPage(0);
            OpenTextBox();
        }

        // Restarts the tutorial.
        public void RestartTutorial()
        {
            // Gets the pages from the text box.
            List<Page> pages = textBox.textBox.pages;

            // Ends the tutorial, sets the textbox pages, and starts the tutorial again.
            EndTutorial();
            textBox.textBox.pages = pages;
            StartTutorial();
        }

        // Ends the tutorial.
        public void EndTutorial()
        {
            // If the tutorial is running, end it.
            if(IsTutorialRunning())
            {
                // Sets to the last page and closes the text box.
                textBox.textBox.SetPage(textBox.textBox.GetPageCount() - 1);
                CloseTextBox();
            }
        }

        // Called when a tutorial is started.
        public void OnTutorialStart()
        {
            // ...
        }

        // Called when a tutorail ends.
        public void OnTutorialEnd()
        {
            // ...
        }

        // TEXT BOX
        // Loads pages for the textbox.
        public void LoadPages(ref List<Page> pages, bool clearPages)
        {
            // If the pages should be cleared.
            if (clearPages)
                textBox.textBox.ClearPages();

            // Adds pages to the end of the text box.
            textBox.textBox.pages.AddRange(pages);

        }

        // Opens Text Box
        public void OpenTextBox()
        {
            textBox.textBox.Open();
        }

        // Closes the Text Box
        public void CloseTextBox()
        {
            textBox.textBox.Close();
        }

        // Text box operations.
        // Called when the text box is opened.
        private void OnTextBoxOpened()
        {
            // These should be handled by the pages.
            // Hides the diagram by default.
            // HideDiagram();

            // The tutorial has started.
            tutorials.OnTutorialStart();
        }

        // Called when the text box is closed.
        private void OnTextBoxClosed()
        {
            // ...
        }

        // Called when the text box is finished.
        private void OnTextBoxFinished()
        {
            // Remove all the pages.
            textBox.textBox.ClearPages();

            // These should be handled by the pages.
            // // Clear the diagram and hides it.
            // ClearDiagram();
            // HideDiagram();

            // The tutorial has ended.
            tutorials.OnTutorialEnd();
        }

        // Diagram
        // Sets the diagram's visibility.
        public void SetDiagramVisibility(bool visible)
        {
            textBoxDiagram.SetActive(visible);
        }

        // Show the diagram.
        public void ShowDiagram()
        {
            SetDiagramVisibility(true);
        }

        // Hide the diagram.
        public void HideDiagram()
        {
            SetDiagramVisibility(false);
        }

        // Clears the diagram.
        public void ClearDiagram()
        {
            // Clear out the sprite.
            textBoxDiagramImage.sprite = alpha0Sprite;
        }

        // DIAGRAM IMAGES
        // Set diagram image by type.
        public void SetDiagramImageByTutorialType(Tutorials.tutorialType tutorial)
        {
            // ...
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