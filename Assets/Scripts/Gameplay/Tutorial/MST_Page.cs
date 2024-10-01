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
        // The language key for the page.
        public string languageKey = string.Empty;

        // Adds a page.
        public MST_Page() : base()
        {
            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Adds a page with text.
        public MST_Page(string text) : base(text)
        {
            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Adds a page with text and a speak key.
        public MST_Page(string text, string languageKey) : base(text)
        {
            // Sets the language key and translates the text.
            SetLanguageText(languageKey);

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Translates the text using the language key.
        public void SetLanguageText()
        {
            // If the LOL Manager is instantiated...
            if(LOLManager.Instantiated)
            {
                string newText = LOLManager.Instance.GetLanguageText(languageKey);
                text = newText;
            }
        }

        // Sets the language key and translates it.
        public void SetLanguageText(string newLangKey)
        {
            languageKey = newLangKey;
            SetLanguageText();
        }

        // Speaks the text for the tutorial page.
        public void SpeakText()
        {
            // If the LOL SDK is initialized, and TTS is on.
            if (LOLManager.IsLOLSDKInitialized() && GameSettings.Instance.UseTextToSpeech)
            {
                LOLManager.Instance.SpeakText(languageKey);
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