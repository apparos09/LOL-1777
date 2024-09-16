using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The data for a stage.
    [System.Serializable]
    public class StageData
    {
        // The stage time.
        public float stageTime;

        // The stage score.
        public float stageScore;

        // The highest combo.
        public int highestCombo;
    }

    // The stage.
    public class Stage : MonoBehaviour
    {
        // The stage manager.
        public StageManager manager;

        // Start is called before the first frame update
        void Start()
        {
            // If the manager is not set, set it.
            if (manager == null)
                manager = StageManager.Instance;

            // Sets the stage.
            if(manager.stage == null)
                manager.stage = this;
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}