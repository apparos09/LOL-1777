using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_MST
{
    // Game audio for the MST project.
    public class MST_GameAudio : GameAudio
    {
        [Header("MST")]

        // If 'true', the audio sources for the UI are automatically set.
        private bool autoSetUIAudio = true;

        // The button (UI) SFX.
        public AudioClip buttonUISfx;

        // The slider (UI) SFX.
        public AudioClip sliderUISfx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Makes sure all button, slider, and toggles are set.
            // Only replaces the audio if the values are null.
            // Take out this autoset if you're going to manually set things.
            if(autoSetUIAudio)
            {
                // Finds all the ui element audio objects.
                UIElementAudio[] uIElementAudios = FindObjectsOfType<UIElementAudio>(true);

                // Goes through the list,and sets the audio source.
                foreach (UIElementAudio uiElementAudio in uIElementAudios)
                {
                    // If the audio source isn't set, set it.
                    if (uiElementAudio.audioSource == null)
                        uiElementAudio.audioSource = sfxUISource;
                }
            }

            // Makes sure the audio is adjusted to the current settings.
            GameSettings.Instance.AdjustAllAudioLevels();
        }

        // Plays the button menu SFX.
        public void PlayButtonUISfx()
        {
            PlaySoundEffectUI(buttonUISfx);
        }

        // Plays the slider menu SFX.
        public void PlaySliderUISfx()
        {
            PlaySoundEffectUI(sliderUISfx);
        }
    }
}