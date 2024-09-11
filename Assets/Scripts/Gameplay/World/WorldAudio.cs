using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The world audio.
    public class WorldAudio : MST_GameAudio
    {
        // Manager
        public WorldManager manager;

        // Start is called before the first frame update
        protected override void Start()
        {
            if (manager == null)
                manager = WorldManager.Instance;
        }

    }
}