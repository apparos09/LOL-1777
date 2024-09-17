using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_MST
{
    // The information window for units.
    public class UnitsInfoMenu : MonoBehaviour
    {
        // THe Units Info Entry
        public struct UnitsInfoEntry
        {
            // The units type.
            public UnitsInfo.unitGroups unitsType;

            // The group the units belong to, and its key.
            public string groupName;
            public string groupNameKey;

            // TODO: need description

            // Description and Speak Key
            public string groupDesc;
            public string groupDescKey;
        }

        // The units info.
        public UnitsInfo unitsInfo;

        // The group name.
        public TMP_Text groupName;

        // The group description.
        public TMP_Text groupDesc;

        // The list of entries.
        public List<UnitsInfoEntry> entries = new List<UnitsInfoEntry>();

        // The entry index.
        public int entryIndex = 0;

        // The previous and next buttons.
        public Button prevButton;
        public Button nextButton;

        // Loads the entries on enable if true.
        // This is false by default so that it's called from Start() first.
        private bool loadEntriesOnEnable = false;

        [Header("Groups")]
        // Measurement groups
        public bool clearedWeightImperial;
        public bool clearedLengthImperial;
        public bool clearedTime;
        
        public bool clearedLengthMetric;
        public bool clearedWeightMetric;
        public bool clearedCapcity;


        // Start is called before the first frame update
        void Start()
        {
            // Gets the units info instance.
            if (unitsInfo == null)
                unitsInfo = UnitsInfo.Instance;

            // Loads the entries, and sets it to do this on enable.
            // OnEnable is triggered before start.
            LoadEntries();
            loadEntriesOnEnable = true;
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // Loads entries on enable.
            if(loadEntriesOnEnable)
                LoadEntries();
        }

        // Generates the units info entry.
        public UnitsInfoEntry GenerateUnitsInfoEntry(UnitsInfo.unitGroups unitsType)
        {
            // A new entry.
            UnitsInfoEntry newEntry = new UnitsInfoEntry();

            newEntry.groupName = unitsInfo.GetUnitsGroupName(unitsType);
            newEntry.groupNameKey = UnitsInfo.GetUnitsGroupNameKey(unitsType);

            // Description
            newEntry.groupDesc = unitsInfo.GetUnitsGroupDescription(unitsType);
            newEntry.groupDescKey = UnitsInfo.GetUnitsGroupDescriptionKey(unitsType);

            return newEntry;
        }

        // Loads the entries.
        public void LoadEntries()
        {
            // Checks if the tutorial is being used.
            bool usingTutorial = false;

            // Checks if the tutorial is instantiated.
            if (Tutorials.Instantiated)
            {
                // If the game settings exist, reference it to see if the tutorial is active.
                // If it doesn't exist, just set it to false.
                usingTutorial = GameSettings.Instantiated ? GameSettings.Instance.UseTutorial : false;
            }

            // // For testing purposes only.
            // usingTutorial = false;

            // Checks if the tutorial is being used.
            if (usingTutorial) // Only enable encountered rules.
            {
                // Gets the tutorial.
                Tutorials tutorials = Tutorials.Instance;

                clearedWeightImperial = tutorials.clearedWeightImperial;
                clearedLengthImperial = tutorials.clearedLengthImperial;
                clearedTime = tutorials.clearedTime;
                clearedLengthMetric = tutorials.clearedLengthMetric;
                clearedWeightMetric = tutorials.clearedWeightMetric;
                clearedCapcity = tutorials.clearedCapcity;
            }
            else // Not being used, so enable all.
            {
                clearedWeightImperial = true;
                clearedLengthImperial = true;
                clearedTime = true;
                clearedLengthMetric = true;
                clearedWeightMetric = true;
                clearedCapcity = true;
            }


            // Clears the list to make sure no old entries are left.
            entries.Clear();

            // Creating Entries
            // Weight (Imperial)
            if(clearedWeightImperial)
            {
                entries.Add(GenerateUnitsInfoEntry(UnitsInfo.unitGroups.weightImperial));
            }

            // Length (Imperial)
            if(clearedLengthImperial)
            {
                entries.Add(GenerateUnitsInfoEntry(UnitsInfo.unitGroups.lengthImperial));
            }

            // Time
            if(clearedTime)
            {
                entries.Add(GenerateUnitsInfoEntry(UnitsInfo.unitGroups.time));
            }

            // Length (Metric)
            if(clearedLengthMetric)
            {
                entries.Add(GenerateUnitsInfoEntry(UnitsInfo.unitGroups.lengthMetric));

            }

            // Weight (Metric)
            if(clearedWeightMetric)
            {
                entries.Add(GenerateUnitsInfoEntry(UnitsInfo.unitGroups.weightMetric));
            }

            // Capacity
            if(clearedCapcity)
            {
                entries.Add(GenerateUnitsInfoEntry(UnitsInfo.unitGroups.capacity));
            }

            // Refreshes the info menu buttons.
            RefreshButtons();

            // Loads an entry.
            SetEntry(0);
        }

        // Refreshes the buttons for the info menu.
        public void RefreshButtons()
        {
            // Enabling/disabling the previous and next buttons.
            if (entries.Count > 1)
            {
                prevButton.interactable = true;
                nextButton.interactable = true;
            }
            else
            {
                prevButton.interactable = false;
                nextButton.interactable = false;
            }
        }

        // Goes to the previous entry.
        public void PreviousEntry()
        {
            // Index
            int index = entryIndex - 1;

            // Boudns check.
            if (index < 0)
                index = entries.Count - 1;

            // Load entry.
            SetEntry(index);
        }

        // Goe to the next entry.
        public void NextEntry()
        {
            // Index
            int index = entryIndex + 1;

            // Boudns check.
            if (index >= entries.Count)
                index = 0;

            // Load entry.
            SetEntry(index);
        }


        // Sets the entry.
        public void SetEntry(int index)
        {
            // No entry to set.
            if (index < 0 || index >= entries.Count)
                return;

            // Sets the index.
            entryIndex = index;

            // Gets the entry.
            UnitsInfoEntry entry = entries[index];

            // TODO: load entry information.

            groupName.text = entry.groupName;
            groupDesc.text = entry.groupDesc;

            // If the LOL Manager has been instantiated.
            if(GameSettings.Instance.UseTextToSpeech)
            {
                // The LOL Manager
                LOLManager lolManager = LOLManager.Instance;

                // If there is a description key, read it.
                if (entry.groupDescKey != "")
                    lolManager.SpeakText(entry.groupDescKey);

            }
        }
    }
}