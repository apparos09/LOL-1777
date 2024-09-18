using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The units table.
    public class UnitsTable : MonoBehaviour
    {
        // The units info object.
        public UnitsInfo unitsInfo;

        // The unit group.
        public UnitsInfo.unitGroups group = UnitsInfo.unitGroups.none;

        // The entries for the units table.
        public List<UnitsTableEntry> entries = new List<UnitsTableEntry>();

        // Start is called before the first frame update
        void Start()
        {
            // Sets the instance.
            if (unitsInfo == null)
                unitsInfo = UnitsInfo.Instance;

            // Gets the components in the children.
            if (entries.Count == 0)
                entries = new List<UnitsTableEntry>(GetComponentsInChildren<UnitsTableEntry>());
        }

        // Sets the group
        public void SetGroup(UnitsInfo.unitGroups newGroup)
        {
            group = newGroup;
            LoadConversions();
        }

        // Loads entries from the provided group.
        public void LoadConversions()
        {
            // If the instance isn't set, set it.
            if (unitsInfo == null)
                unitsInfo = UnitsInfo.Instance;

            // If the group is none, or if the units info is unavailable, clear the entries.
            if (group == UnitsInfo.unitGroups.none)
            {
                // Set the units info if it's not set already.
                if (unitsInfo == null)
                    unitsInfo = UnitsInfo.Instance;

                // Clears the entries.
                ClearEntries();
            }
            else // Load the entries.
            {
                // Gets the conversion list for the provided group.
                // TODO: it's inefficient to get these as copies, but this is to prevent the lists from being edited.
                List<UnitsInfo.UnitsConversion> conversions = unitsInfo.GetGroupConversionListCopy(group);

                // The entry index.
                int entryIndex = 0;

                // Goes through all the conversions and loads them in.
                for(int i = 0; i < conversions.Count && i < entries.Count; i++)
                {
                    entries[i].gameObject.SetActive(true);
                    entries[i].SetConversion(conversions[i]);
                    entryIndex++; // Add to the index.
                }

                // While there are remaining entries, clear them out.
                while(entryIndex < entries.Count)
                {
                    entries[entryIndex].gameObject.SetActive(true);
                    entries[entryIndex].ClearText();
                    entries[entryIndex].gameObject.SetActive(false);

                    entryIndex++;
                }
            }
        }

        // Clears all the entries.
        public void ClearEntries()
        {
            // Clear out all the entries.
            foreach (UnitsTableEntry entry in entries)
            {
                entry.gameObject.SetActive(true);
                entry.ClearText();
                entry.gameObject.SetActive(false);
            }
        }
    }
}