using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_MST
{
    // The speed button.
    public class SpeedButton : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The button.
        public Button button;

        // Symbols
        [Header("Images, Sprites")]

        // The button symbol's image.
        public Image speedSymbolImage;

        // The sprite for slow speed.
        public Sprite slowSpeedSprite;

        // The sprite for normal speed.
        public Sprite normalSpeedSprite;

        // The sprite for fast speed.
        public Sprite fastSpeedSprite;

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Button not set, so get it.
            if (button == null)
                button = GetComponent<Button>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the stage manager.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // Add on click.
            AddOnClick();

            // Makes sure the symbol is set correctly.
            OnClick();
        }

        // Add OnClick Delegate
        private void AddOnClick()
        {
            // If the button has been set.
            if (button != null)
            {
                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    OnClick();
                });
            }
        }

        // Remove OnClick Delegate
        private void RemoveOnClick()
        {
            // Remove the listener for onClick if the button has been set.
            if (button != null)
            {
                button.onClick.RemoveListener(OnClick);
            }
        }

        // Called when the button is clicked.
        private void OnClick()
        {
            // Gets the instance if it's not already set.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // Checks the game speed.
            if(stageManager.IsNormalSpeed()) // Normal
            {
                speedSymbolImage.sprite = normalSpeedSprite;
            }
            else if(stageManager.IsFastSpeed()) // Fast
            {
                speedSymbolImage.sprite = fastSpeedSprite;
            }
            else if(stageManager.IsSlowSpeed()) // Slow
            {
                speedSymbolImage.sprite= slowSpeedSprite;
            }
        }
    }
}