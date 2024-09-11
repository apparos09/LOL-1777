using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // A base for a game audio manager.
    public class GameAudio : MonoBehaviour
    {
        // The audio sources.
        // Background Music
        public AudioSource bgmSource;
        public AudioSourceLooper bgmLooper;

        // Sound Effects
        public AudioSource sfxSource;

        // Voice
        public AudioSource vceSource;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the looper exists.
            if (bgmLooper != null)
            {
                // If the looper's audio source has not been set.
                if (bgmLooper.audioSource == null)
                {
                    // Set the audio source.
                    bgmLooper.audioSource = bgmSource;
                }
            }
        }

        // Plays the provided background music.
        // The arguments 'clipStart' and 'clipEnd' are used for the BGM looper.
        public void PlayBackgroundMusic(AudioClip bgmClip, float clipStart, float clipEnd)
        {
            // If the looper has been set, change it thorugh that.
            if (bgmLooper != null)
            {
                // Stop the audio and set the clip. This puts the audio at its start.
                bgmLooper.StopAudio(true);
                bgmLooper.audioSource.clip = bgmClip;

                // Sets the start and end for the BGM.
                bgmLooper.clipStart = clipStart;
                bgmLooper.clipEnd = clipEnd;

                // Play the BGM through the looper
                bgmLooper.PlayAudio(true);
            }
            else // No looper, so change settings normally.
            {
                // Stops the BGM source and sets the current clip.
                bgmSource.Stop();
                bgmSource.clip = bgmClip;

                // Play the BGM with the normal settings.
                bgmSource.Play();
            }

        }

        // Plays the background music (clipStart and clipEnd are autoset to the start and end of the audio).
        public void PlayBackgroundMusic(AudioClip bgmClip)
        {
            PlayBackgroundMusic(bgmClip, 0, bgmClip.length);
        }

        // Plays the provided background music.
        // If 'stopAudio' is 'true', then the BGM is stopped before playing the one shot.
        public void PlayBackgroundMusicOneShot(AudioClip bgmClip, bool stopCurrAudio = true)
        {
            // If the current audio should be stopped.
            if (stopCurrAudio)
                bgmSource.Stop();

            bgmSource.PlayOneShot(bgmClip);
        }

        // Stops the provided background music.
        public void StopBackgroundMusic()
        {
            bgmSource.Stop();
        }

        // Plays the sound effect.
        public void PlaySoundEffect(AudioClip sfxClip)
        {
            bgmSource.PlayOneShot(sfxClip);
        }

        // Stops the sound effect source.
        public void StopSoundEffect()
        {
            sfxSource.Stop();
        }

        // Plays the voice clip.
        public void PlayVoice(AudioClip vceClip)
        {
            vceSource.PlayOneShot(vceClip);
        }

        // Stops the voice clip.
        public void StopVoice()
        {
            vceSource.Stop();
        }
    }
}