using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The audio for the title screen.
    public class TitleAudio : MST_GameAudio
    {
        // Manager
        public TitleManager manager;

        // Start is called before the first frame update
        protected override void Start()
        {
            if (manager == null)
                manager = TitleManager.Instance;
        }

    }
}