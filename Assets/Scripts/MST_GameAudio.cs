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

        // The button (menu) SFX.
        public AudioClip buttonMenuSfx;

        // The slider (menu) SFX.
        public AudioClip sliderMenuSfx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Makes sure all button, slider, and toggles are set.
            // Only replaces the audio if the values are null.
            // Take out this autoset if you're going to manually set things.
            if(autoSetUIAudio)
            {
                // Button Audios
                ButtonAudio[] buttonAudios = FindObjectsOfType<ButtonAudio>(true);

                foreach(ButtonAudio buttonAudio in buttonAudios)
                {
                    if(buttonAudio.audioSource == null)
                        buttonAudio.audioSource = sfxUISource;
                }

                // Toggle Audios
                ToggleAudio[] toggleAudios = FindObjectsOfType<ToggleAudio>(true);

                foreach (ToggleAudio toggleAudio in toggleAudios)
                {
                    if (toggleAudio.audioSource == null)
                        toggleAudio.audioSource = sfxUISource;
                }

                // Slider Audios
                SliderAudio[] sliderAudios = FindObjectsOfType<SliderAudio>(true);

                foreach (SliderAudio sliderAudio in sliderAudios)
                {
                    if (sliderAudio.audioSource == null)
                        sliderAudio.audioSource = sfxUISource;
                }
            }
        }

        // Plays the button menu SFX.
        public void PlayButtonUISfx()
        {
            PlaySoundEffectUI(buttonMenuSfx);
        }

        // Plays the slider menu SFX.
        public void PlaySliderUISfx()
        {
            PlaySoundEffectUI(sliderMenuSfx);
        }
    }
}