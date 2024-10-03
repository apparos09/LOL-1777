using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // the asynchronous scene loader visual
    public class AsyncSceneLoadVisual : MonoBehaviour
    {
        // Scene loader.
        public Slider slider;

        // Load operation.
        public AsyncSceneLoader loader;

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // Update is called once per frame
        void Update()
        {
            // Checks if the loader is loading.
            if (loader.IsLoading)
            {
                // Changes the slider.
                if(slider != null)
                {
                    slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, loader.GetProgressLoading());
                }
            }
        }
    }
}