using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_MST
{
    // Game audio for the MST project.
    public class MST_GameAudio : GameAudio
    {
        // The button (menu) SFX.
        public AudioClip buttonMenuSfx;

        // The slider (menu) SFX.
        public AudioClip sliderMenuSfx;

        // Plays the button menu SFX.
        public void PlayButtonMenuSfx()
        {
            PlaySoundEffect(buttonMenuSfx);
        }

        // Plays the slider menu SFX.
        public void PlaySliderMenuSfx()
        {
            PlaySoundEffect(sliderMenuSfx);
        }
    }
}