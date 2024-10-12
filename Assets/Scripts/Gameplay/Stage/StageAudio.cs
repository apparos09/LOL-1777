using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The stage audio.
    public class StageAudio : MST_GameAudio
    {
        // The singleton instance.
        private static StageAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Stage")]
        // Manager
        public StageManager manager;

        [Header("Stage/BGMs")]
        // The stage BGMs. BGM 0 is the debug BGM.
        public AudioClip stageBgm00;
        public AudioClip stageBgm01;
        public AudioClip stageBgm02;
        public AudioClip stageBgm03;

        // The stage results bgms.
        public AudioClip stageResultsBgm;

        [Header("Stage/JNGs")]

        // The stage cleared jingle.
        public AudioClip stageClearedJng;

        // The stage failed jingle.
        public AudioClip stageFailedJng;

        // Constructor
        private StageAudio()
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
                instanced = true;
            }
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if (manager == null)
                manager = StageManager.Instance;

            // If nothing is set, play the default stage bgm.
            if (bgmSource.clip == null && stageBgm00 != null)
            {
                PlayBackgroundMusic(stageBgm00);
            }
        }

        // Gets the instance.
        public static StageAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<StageAudio>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("StageAudio (singleton)");
                        instance = go.AddComponent<StageAudio>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Plays the stage BGM using the provided number.
        public void PlayStageBgm(int bgmNumber)
        {
            // The clip to be played.
            AudioClip clip;

            // Checks the BGM number to know which one to play.
            switch(bgmNumber)
            {
                default:
                case 0:
                    clip = stageBgm00;
                    break;

                case 1:
                    clip = stageBgm01;
                    break;

                case 2:
                    clip = stageBgm02;
                    break;

                case 3:
                    clip = stageBgm03;
                    break;
            }

            // Play the music.
            PlayBackgroundMusic(clip);
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
