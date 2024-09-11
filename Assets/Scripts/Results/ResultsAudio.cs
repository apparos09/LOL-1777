using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The results audio.
    public class ResultsAudio : MST_GameAudio
    {
        // Manager
        public ResultsManager manager;

        // Start is called before the first frame update
        protected override void Start()
        {
            if (manager == null)
                manager = ResultsManager.Instance;
        }

    }
}