using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The units info.
    public class UnitsInfo : MonoBehaviour
    {
        // TODO: implement units.

        // The measurement units.
        public enum units { none, weightImperial, lengthImperial, time, lengthMetric, weightMetric, capacity }

        // The singleton instance.
        private static UnitsInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Awake is called when the script is being loaded
        void Awake()
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
        void Start()
        {

        }

        // Gets the instance.
        public static UnitsInfo Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<UnitsInfo>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Units Info (singleton)");
                        instance = go.AddComponent<UnitsInfo>();
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

        // Gets the units as a string.
        public string UnitsToString(units unitsType)
        {
            // TODO: implement, and include shorthands.

            // Checks the units type.
            switch (unitsType)
            {
                case units.weightImperial: // Weight Imperial
                    break;

                case units.lengthImperial: // Length Imperial
                    break;

                case units.time: // Time
                    break;

                case units.lengthMetric: // Metric
                    break;

                case units.weightMetric: // Weight
                    break;

                case units.capacity: // Capacity
                    break;
            }

            return string.Empty;
        }

        // Gets the units group name.
        public string GetUnitsGroupName(units unitsType)
        {
            // TODO: implement.

            return "";
        }

        // Gets the units group name key.
        public string GetUnitsGroupNameKey(units unitsType)
        {
            // TODO: implement.

            return "";
        }

        // Gets the units group description.
        public string GetUnitsGroupDescription(units unitsType)
        {
            // TODO: implement.

            return "";
        }

        // Gets the units group description key.
        public string GetUnitsGroupDescriptionKey(units unitsType)
        {
            // TODO: implement.

            return "";
        }

        // Update is called once per frame
        void Update()
        {

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