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
        public StageManager stageManager;

        // The meteor spawn point minimum.
        public GameObject meteorSpawnPointMin;

        // The meteor spawn point maximum.
        public GameObject meteorSpawnPointMax;

        // The parent object for a meteor.
        public GameObject meteorParent;

        // Start is called before the first frame update
        void Start()
        {
            // If the manager is not set, set it.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // Sets the stage.
            if(stageManager.stage == null)
                stageManager.stage = this;
        }

        // Generrates a spawn point for a meteor.
        public Vector3 GenerateMeteorSpawnPoint()
        {
            // If either spawn point is empty, return a zero vector.
            if (meteorSpawnPointMin == null || meteorSpawnPointMax == null)
            {
                return Vector3.zero;
            }
            // If the max spawn point is missing, return the minimum.
            else if (meteorSpawnPointMin != null && meteorSpawnPointMax == null)
            {
                return meteorSpawnPointMin.transform.position;
            }
            // If the min spawn point is missing, return the maximum.
            else if(meteorSpawnPointMin == null && meteorSpawnPointMax != null)
            {
                return meteorSpawnPointMax.transform.position;
            }
            else // Randomize the position between the min and max.
            {
                // Gets the min and max.
                Vector3 min = meteorSpawnPointMin.transform.position;
                Vector3 max = meteorSpawnPointMax.transform.position;

                // If the values are the same, return the value.
                if (min == max)
                    return min;

                // Generates the resulting position.
                Vector3 result = new Vector3();
                result.x = Random.Range(min.x, max.x);
                result.y = Random.Range(min.y, max.y);
                result.z = Random.Range(min.z, max.z);

                // Returns the result.
                return result;
            }
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}