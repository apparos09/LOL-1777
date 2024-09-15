using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_MST
{
    // A page for the MST namepsace.
    public class MST_Page : Page
    {
        // The speak key for the page.
        public string speakKey = string.Empty;

        // Adds a page.
        public MST_Page() : base()
        {
            // ...
        }

        // Adds a page with text.
        public MST_Page(string text) : base(text)
        {
           // ...
        }

        // Adds a page with text and a speak key.
        public MST_Page(string text, string speakKey) : base(text)
        {
            this.speakKey = speakKey;
        }

        // Speaks the text for the tutorial page.
        public void SpeakText()
        {
            // If the LOL SDK is initialized, and TTS is on.
            if (LOLManager.IsLOLSDKInitialized() && GameSettings.Instance.UseTextToSpeech)
            {
                LOLManager.Instance.SpeakText(speakKey);
            }
        }

        // Adds the speak text callback.
        public void AddSpeakTextCallback()
        {
            OnPageOpenedAddCallback(SpeakText);
        }

        // Removes the speak text callback.
        public void RemoveSpeakTextCallback()
        {
            OnPageOpenedRemoveCallback(SpeakText);
        }

    }
}