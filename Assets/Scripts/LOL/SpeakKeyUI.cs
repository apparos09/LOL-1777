using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // Speaks text as part of the UI.
    public class SpeakKeyUI : MonoBehaviour
    {
        // The game settings.
        public GameSettings gameSettings;

        // The LOL manager.
        public LOLManager lolManager;

        // Start is called before the first frame update
        void Start()
        {
            // Settings
            if (gameSettings == null)
                gameSettings = GameSettings.Instance;

            // LOL Manager
            if (lolManager == null)
                lolManager = LOLManager.Instance;
        }

        // Speaks text using the provided key.
        public void SpeakText(string key)
        {
            // Checks if the instances exist.
            if (GameSettings.Instantiated && LOLManager.Instantiated)
            {
                // Checks if TTS should be used.
                if (GameSettings.Instance.UseTextToSpeech)
                {
                    // Grabs the LOL Manager to trigger text-to-speech.
                    LOLManager lolManager = LOLManager.Instance;
                    lolManager.textToSpeech.SpeakText(key);
                }
            }
        }
    }
}
