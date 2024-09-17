using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The stage audio.
    public class StageAudio : MST_GameAudio
    {
        // Manager
        public StageManager manager;

        // Start is called before the first frame update
        protected override void Start()
        {
            if (manager == null)
                manager = StageManager.Instance;
        }

    }
}