using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // Speaks text as part of the UI.
    public class SpeakKeyUI : MonoBehaviour
    {
        // Speaks text using the provided key.
        public void SpeakText(string key)
        {
            // Checks if the instances exist.
            if (GameSettings.Instantiated && LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                // Gets the instances.
                GameSettings gameSettings = GameSettings.Instance;
                LOLManager lolManager = LOLManager.Instance;

                // Checks if TTS should be used.
                if (gameSettings.UseTextToSpeech)
                {
                    lolManager.textToSpeech.SpeakText(key);
                }
            }
        }
    }
}
