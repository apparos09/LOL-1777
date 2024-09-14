using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The units info.
    public class UnitsInfo : MonoBehaviour
    {
        // The measurement units.
        public enum unitGroups { none, lengthImperial, weightImperial, time, lengthMetric, weightMetric, capacity }

        // The weight units.
        public enum weightUnits { unknown, pound, ounce, milligram, gram, kilogram };

        // The length units.
        public enum lengthUnits { unknown, inch, foot, yard, millimeter, centimeter, decimeter, meter, kilometer }

        // The time units.
        public enum timeUnits { unknown, second, minute, hour }

        // The capacity units.
        public enum capacityUnits { unknown, liter, milliliter }

        // Instance
        // The singleton instance.
        private static UnitsInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;


        // Unit group pairings (these are instantiated in the start function)
        // Length (Imperial)
        private List<lengthUnits> lengthImperialList;

        // Weight
        private List<weightUnits> weightImperialList;

        // Time
        private List<timeUnits> timeList;

        // Length (Metric)
        private List<lengthUnits> lengthMetricList;

        // Weight (Metric)
        private List<weightUnits> weightMetricList;

        // Capacity
        private List<capacityUnits> capacityList;

        // TODO: change descriptions?

        // NAMES AND DESCRIPTIONS
        // Unit group names, descriptions, and keys.
        // Length Imperial
        private string lengthImperialName = "Length (Imperial)";
        public const string LENGTH_IMPERIAL_NAME_KEY = "unt_lengthImperial_nme";
        private string lengthImperialDesc = "Inches (in), Feet (ft), Yards (yd)";
        public const string LENGTH_IMPERIAL_DESC_KEY = "unt_lengthImperial_dsc";

        // Weight Imperial
        private string weightImperialName = "Weight (Imperial)";
        public const string WEIGHT_IMPERIAL_NAME_KEY = "unt_weightImperial_nme";
        private string weightImperialDesc = "Pounds (lb), Ounces (oz)";
        public const string WEIGHT_IMPERIAL_DESC_KEY = "unt_weightImperial_dsc";

        // Time
        private string timeName = "Time";
        public const string TIME_NAME_KEY = "unt_time_nme";
        private string timeDesc = "Seconds (secs), Minutes (mins), Hours (hrs)";
        public const string TIME_DESC_KEY = "unt_time_dsc";

        // Length Metric
        private string lengthMetricName = "Length (Metric)";
        public const string LENGTH_METRIC_NAME_KEY = "unt_lengthMetric_nme";
        private string lengthMetricDesc = "Millimeters (mm), Centimeters (cm), Decimeters (dm), Meters (m), Kilometers (km)";
        public const string LENGTH_METRIC_DESC_KEY = "unt_lengthMetric_dsc";

        // Weight Metric
        private string weightMetricName = "Weight (Metric)";
        public const string WEIGHT_METRIC_NAME_KEY = "unt_weightMetric_nme";
        private string weightMetricDesc = "Milligrams (mg), Grams (g) Kilograms (kg)";
        public const string WEIGHT_METRIC_DESC_KEY = "unt_weightMetric_dsc";

        // Capacity
        private string capacityName = "Capacity";
        public const string CAPACITY_NAME_KEY = "unt_capacity_nme";
        private string capacityDesc = "Liters (l), Milliliters (ml)";
        public const string CAPACITY_DESC_KEY = "unt_capacity_dsc";

        // Units names and descriptions
        private string inchName = "Inches";
        private string inchShorthand = "in";

        private string footName = "Feet";
        private string footShorthand = "ft";

        private string yardName = "Yards";
        private string yardShorthand = "yd";

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
            // Length (Imperial)
            lengthImperialList = new List<lengthUnits>
            {
                lengthUnits.inch,
                lengthUnits.foot,
                lengthUnits.yard
            };

            // Weight
            weightImperialList = new List<weightUnits>
            {
                weightUnits.pound,
                weightUnits.ounce
            };

            // Time
            timeList = new List<timeUnits>()
            {
                timeUnits.second,
                timeUnits.minute,
                timeUnits.hour
            };

            // Length (Metric)
            lengthMetricList = new List<lengthUnits>() 
            {
                lengthUnits.millimeter,
                lengthUnits.centimeter,
                lengthUnits.meter,
                lengthUnits.decimeter,
                lengthUnits.kilometer
            };

            // Weight (Metric)
            weightMetricList = new List<weightUnits>()
            {
                weightUnits.milligram,
                weightUnits.gram,
                weightUnits.kilogram
            };

            // Capacity
            capacityList = new List<capacityUnits>()
            { 
                capacityUnits.milliliter,
                capacityUnits.liter
            };

            // TOOD: translate the names
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

        // UNIT GROUPS AND DESCRIPTIONS
        // Gets the units group name.
        public string GetUnitsGroupName(unitGroups unitsType)
        {
            // The result.
            string result = "";

            // Checks the units type.
            switch (unitsType)
            {
                case unitGroups.weightImperial: // Weight Imperial
                    result = weightImperialName;
                    break;

                case unitGroups.lengthImperial: // Length Imperial
                    result = lengthImperialName;
                    break;

                case unitGroups.time: // Time
                    result = timeName;
                    break;

                case unitGroups.lengthMetric: // Metric
                    result = lengthMetricName;
                    break;

                case unitGroups.weightMetric: // Weight
                    result = weightMetricName;
                    break;

                case unitGroups.capacity: // Capacity
                    result = capacityName;
                    break;
            }

            // Returns the result.
            return result;
        }

        // Gets the units group name key.
        public static string GetUnitsGroupNameKey(unitGroups unitsType)
        {
            // The result.
            string result = "";

            // Checks the units type.
            switch (unitsType)
            {
                case unitGroups.weightImperial: // Weight Imperial
                    result = WEIGHT_IMPERIAL_NAME_KEY;
                    break;

                case unitGroups.lengthImperial: // Length Imperial
                    result = LENGTH_IMPERIAL_NAME_KEY;
                    break;

                case unitGroups.time: // Time
                    result = TIME_NAME_KEY;
                    break;

                case unitGroups.lengthMetric: // Metric
                    result = LENGTH_METRIC_NAME_KEY;
                    break;

                case unitGroups.weightMetric: // Weight
                    result = WEIGHT_METRIC_NAME_KEY;
                    break;

                case unitGroups.capacity: // Capacity
                    result = CAPACITY_NAME_KEY;
                    break;
            }

            // Returns the result.
            return result;
        }

        // Gets the units group name.
        public string GetUnitsGroupDescription(unitGroups unitsType)
        {
            // The result.
            string result = "";

            // Checks the units type.
            switch (unitsType)
            {
                case unitGroups.weightImperial: // Weight Imperial
                    result = weightImperialDesc;
                    break;

                case unitGroups.lengthImperial: // Length Imperial
                    result = lengthImperialDesc;
                    break;

                case unitGroups.time: // Time
                    result = timeDesc;
                    break;

                case unitGroups.lengthMetric: // Metric
                    result = lengthMetricDesc;
                    break;

                case unitGroups.weightMetric: // Weight
                    result = weightMetricDesc;
                    break;

                case unitGroups.capacity: // Capacity
                    result = capacityDesc;
                    break;
            }

            // Returns the result.
            return result;
        }

        // Gets the units group description key.
        public static string GetUnitsGroupDescriptionKey(unitGroups unitsType)
        {
            // The result.
            string result = "";

            // Checks the units type.
            switch (unitsType)
            {
                case unitGroups.weightImperial: // Weight Imperial
                    result = WEIGHT_IMPERIAL_DESC_KEY;
                    break;

                case unitGroups.lengthImperial: // Length Imperial
                    result = LENGTH_IMPERIAL_DESC_KEY;
                    break;

                case unitGroups.time: // Time
                    result = TIME_DESC_KEY;
                    break;

                case unitGroups.lengthMetric: // Metric
                    result = LENGTH_METRIC_DESC_KEY;
                    break;

                case unitGroups.weightMetric: // Weight
                    result = WEIGHT_METRIC_NAME_KEY;
                    break;

                case unitGroups.capacity: // Capacity
                    result = CAPACITY_DESC_KEY;
                    break;
            }

            // Returns the result.
            return result;
        }


        // GROUP CHECKS
        // Checks if the provided unit is an imperial weight.
        public bool IsWeightImperial(weightUnits units)
        {
            bool result = weightImperialList.Contains(units);
            return result;
        }

        // Checks if the provided unit is a metric weight.
        public bool IsWeightMetric(weightUnits units)
        {
            bool result = weightMetricList.Contains(units);
            return result;
        }

        // Checks if the provided unit is an imperial length.
        public bool IsLengthImperial(lengthUnits units)
        {
            bool result = lengthImperialList.Contains(units);
            return result;
        }

        // Checks if the provided unit is a metric length.
        public bool IsLengthMetric(lengthUnits units)
        {
            bool result = lengthMetricList.Contains(units);
            return result;
        }


        // CONVERSIONS
        // Converts the length measurement units.
        // Returns 0 if the conversion cannot be calculated.
        public float ConvertLengthUnits(float value, lengthUnits input, lengthUnits output)
        {
            // The modifier.
            float modifier = 0.0F;

            // Checks the input and output to see what the modifier should be.
            switch (input)
            {
                case lengthUnits.inch:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 1.0F;
                            break;

                        case lengthUnits.foot:
                            modifier = 1.0F / 12.0F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1.0F / 36.0F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 25.40F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 2.54F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 1.0F / 3.937F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1.0F / 39.37F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F / 39370.0F;
                            break;

                    }
                    break;

                case lengthUnits.foot:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 12.0F;
                            break;

                        case lengthUnits.foot:
                            modifier = 1.0F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1.0F / 3.0F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 304.8F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 30.48F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 3.048F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1.0F / 3.281F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F / 3281F;
                            break;

                    }
                    break;

                case lengthUnits.yard:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 36.0F;
                            break;

                        case lengthUnits.foot:
                            modifier = 3.0F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1.0F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 914.4F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 91.44F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 9.144F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1.0F / 1.094F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F / 1094F;
                            break;

                    }
                    break;

                case lengthUnits.millimeter:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 1.0F / 25.4F;
                            break;

                        case lengthUnits.foot:
                            modifier = 1.0F / 304.8F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1.0F / 914.4F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 1.0F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 1.0F / 10.0F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 1.0F / 100.0F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1.0F / 1000.0F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F / 1000000.0F;
                            break;

                    }
                    break;

                case lengthUnits.centimeter:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 1.0F / 2.54F;
                            break;

                        case lengthUnits.foot:
                            modifier = 1.0F / 30.48F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1.0F / 91.44F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 10.0F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 1.0F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 1.0F / 10.0F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1.0F / 100.0F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F / 100000.0F;
                            break;

                    }
                    break;

                case lengthUnits.decimeter:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 3.937F;
                            break;

                        case lengthUnits.foot:
                            modifier = 1.0F / 3.048F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1.0F / 9.144F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 100.0F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 10.0F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 1.0F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1.0F / 10.0F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F / 10000.0F;
                            break;

                    }
                    break;

                case lengthUnits.meter:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 39.37F;
                            break;

                        case lengthUnits.foot:
                            modifier = 3.281F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1.094F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 1000.0F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 100.0F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 10.0F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1.0F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F / 1000.0F;
                            break;

                    }
                    break;

                case lengthUnits.kilometer:

                    // Modifier
                    switch (output)
                    {
                        case lengthUnits.inch:
                            modifier = 39370.0F;
                            break;

                        case lengthUnits.foot:
                            modifier = 3281.0F;
                            break;

                        case lengthUnits.yard:
                            modifier = 1094.0F;
                            break;

                        case lengthUnits.millimeter:
                            modifier = 1000000.0F;
                            break;

                        case lengthUnits.centimeter:
                            modifier = 100000.0F;
                            break;

                        case lengthUnits.decimeter:
                            modifier = 10000.0F;
                            break;

                        case lengthUnits.meter:
                            modifier = 1000.0F;
                            break;

                        case lengthUnits.kilometer:
                            modifier = 1.0F;
                            break;

                    }
                    break;

            }

            // Generates the result.
            float result = value * modifier;

            // Returns the result.
            return result;
        }

        // Converts the weight measurement units.
        // Returns 0 if the conversion cannot be calculated.
        public float ConvertWeightUnits(float value, weightUnits input, weightUnits output)
        {
            // The modifier.
            float modifier = 0.0F;

            // Checks the input and output to see what the modifier should be.
            switch (input)
            {
                case weightUnits.pound:

                    // Modifier
                    switch (output)
                    {
                        case weightUnits.pound:
                            modifier = 1.0F;
                            break;

                        case weightUnits.ounce:
                            modifier = 16.0F;
                            break;

                        case weightUnits.milligram:
                            modifier = 453600.0F;
                            break;

                        case weightUnits.gram:
                            modifier = 453.60F;
                            break;

                        case weightUnits.kilogram:
                            modifier = 1.0F / 2.205F;
                            break;

                    }
                    break;

                case weightUnits.ounce:

                    // Modifier
                    switch (output)
                    {
                        case weightUnits.pound:
                            modifier = 1.0F / 16.0F;
                            break;

                        case weightUnits.ounce:
                            modifier = 1.0F;
                            break;

                        case weightUnits.milligram:
                            modifier = 28350.0F;
                            break;

                        case weightUnits.gram:
                            modifier = 28.35F;
                            break;

                        case weightUnits.kilogram:
                            modifier = 1.0F / 35.274F;
                            break;

                    }
                    break;

                case weightUnits.milligram:

                    // Modifier
                    switch (output)
                    {
                        case weightUnits.pound:
                            modifier = 1.0F / 453600.0F;
                            break;

                        case weightUnits.ounce:
                            modifier = 1.0F / 28350.0F;
                            break;

                        case weightUnits.milligram:
                            modifier = 1.0F;
                            break;

                        case weightUnits.gram:
                            modifier = 1.0F / 1000.0F;
                            break;

                        case weightUnits.kilogram:
                            modifier = 1.0F / 1000000.0F;
                            break;


                    }
                    break;

                case weightUnits.gram:

                    // Modifier
                    switch (output)
                    {
                        case weightUnits.pound:
                            modifier = 1.0F / 453.6F;
                            break;

                        case weightUnits.ounce:
                            modifier = 1.0F / 28.35F;
                            break;

                        case weightUnits.milligram:
                            modifier = 1000.0F;
                            break;

                        case weightUnits.gram:
                            modifier = 1.0F;
                            break;

                        case weightUnits.kilogram:
                            modifier = 1.0F / 1000.0F;
                            break;


                    }
                    break;

                case weightUnits.kilogram:

                    // Modifier
                    switch (output)
                    {
                        case weightUnits.pound:
                            modifier = 2.205F;
                            break;

                        case weightUnits.ounce:
                            modifier = 35.274F;
                            break;

                        case weightUnits.milligram:
                            modifier = 1000000.0F;
                            break;

                        case weightUnits.gram:
                            modifier = 1000.0F;
                            break;

                        case weightUnits.kilogram:
                            modifier = 1.0F;
                            break;


                    }
                    break;

            }

            // Generates the result.
            float result = value * modifier;

            // Returns the result.
            return result;
        }

        // Converts the time measurement units.
        // Returns 0 if the conversion cannot be calculated.
        public float ConvertTimeUnits(float value, timeUnits input, timeUnits output)
        {
            // The modifier.
            float modifier = 0.0F;

            // Checks the input and output to see what the modifier should be.
            switch (input)
            {
                case timeUnits.second:
                    
                    // Modifier
                    switch (output)
                    {
                        case timeUnits.second:
                            modifier = 1.0F;
                            break;

                        case timeUnits.minute:
                            modifier = 1.0F / 60.0F;
                            break;

                        case timeUnits.hour:
                            modifier = 1.0F / 3600.0F;
                            break;
                    }

                    break;

                case timeUnits.minute:
                    // Modifier
                    switch (output)
                    {
                        case timeUnits.second:
                            modifier = 60.0F;
                            break;

                        case timeUnits.minute:
                            modifier = 1.0F;
                            break;

                        case timeUnits.hour:
                            modifier = 1.0F / 60.0F;
                            break;
                    }
                    break;

                case timeUnits.hour:
                    // Modifier
                    switch (output)
                    {
                        case timeUnits.second:
                            modifier = 3600.0F;
                            break;

                        case timeUnits.minute:
                            modifier = 60.0F;
                            break;

                        case timeUnits.hour:
                            modifier = 1.0F;
                            break;
                    }
                    break;
            }

            // Generates the result.
            float result = value * modifier;
            
            // Returns the result.
            return result;

        }

        // Converts the capacity measurement units.
        // Returns 0 if the conversion cannot be calculated.
        public float ConvertCapacityUnits(float value, capacityUnits input, capacityUnits output)
        {
            // The modifier.
            float modifier = 0.0F;

            // Checks the input and output to see what the modifier should be.
            switch (input)
            {
                case capacityUnits.milliliter:

                    // Modifier
                    switch (output)
                    {
                        case capacityUnits.milliliter:
                            modifier = 1.0F;
                            break;

                        case capacityUnits.liter:
                            modifier = 1000.0F;
                            break;


                    }                    
                    break;

                case capacityUnits.liter:
                    // Modifier
                    switch (output)
                    {
                        case capacityUnits.milliliter:
                            modifier = 1.0F / 1000.0F;
                            break;

                        case capacityUnits.liter:
                            modifier = 1.0F;
                            break;


                    }
                    break;

            }

            // Generates the result.
            float result = value * modifier;

            // Returns the result.
            return result;
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