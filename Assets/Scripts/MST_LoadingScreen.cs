using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_MST
{
    // The loading screen.
    public class MST_LoadingScreen : MonoBehaviour
    {
        // The scene that is loaded when the loading screen starts.
        public string nextScene = "";

        // The loader for loading scenes asynchronously.
        public util.AsyncSceneLoader asyncLoader;

        // Loads the next scene on Loading Screen - Opening End.
        [Tooltip("If true, the next scene is loaded at the end of the opening animation.")]
        public bool loadNextScene = true;

        // Loads the scene asynchronously if true.
        public bool loadSceneAsync = true;

        [Header("Animation")]
        // The animator for the loading screen.
        public Animator animator;

        // Opening animation.
        public string openingAnim = "Loading Screen - Opening Animation";

        // Closing animation.
        public string closingAnim = "Loading Screen - Closing Animation";

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // If the animator is not set, set it.
            if (animator == null)
                animator = GetComponent<Animator>();

            // Gets the asynchronous loader.
            if(asyncLoader)
            {
                asyncLoader = GetComponent<AsyncSceneLoader>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // Plays the loading screen opening animation.

        public void PlayLoadingScreenOpeningAnimation()
        {
            animator.Play(openingAnim);
        }

        // Loading Screen - Opening Start
        public void OnLoadingScreenOpeningStart()
        {

        }

        // Loading Screen - Opening End
        public void OnLoadingScreenOpeningEnd()
        {
            // If the next scene should be loaded, load it.
            if(loadNextScene)
            {
                // Checks if the scene should be loaded asynchronously or not.
                if(loadSceneAsync) // Async
                {
                    asyncLoader.LoadScene(nextScene);
                }
                else // Sync
                {
                    SceneManager.LoadScene(nextScene);
                }
            }
        }

        // Plays the loading screen closing animation.

        public void PlayLoadingScreenClosingAnimation()
        {
            animator.Play(closingAnim);
        }

        // Loading Screen - Closing Start
        public void OnLoadingScreenClosingStart()
        {

        }

        // Loading Screen - Closing End
        public void OnLoadingScreenClosingEnd()
        {

        }
    }
}