using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The target for meteors.
    public class MeteorTarget : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The meteor being targeted.
        public Meteor meteor;

        // The target's movement speed.
        [Tooltip("How long it takes for the target to move to the meteor's position.")]
        public float trackSpeed = 30.0F;

        // If 'true', the exact meteor position is tracked.
        public bool trackExactPos = false;

        // TODO: add animation.

        // Start is called before the first frame update
        void Start()
        {
            // Set the instance.
            if (stageManager == null)
                stageManager = StageManager.Instance;
        }

        // Called when the meteor is targeted.
        private void OnMeteorTargeted()
        {
            // TODO: implement.
        }

        // Removes the target for the meteor.
        public void RemoveTarget()
        {
            meteor = null;
            trackExactPos = false;
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: move to late update.
            // Meteor is set.
            if(meteor != null)
            {
                // Should exact position be tracked?
                if(trackExactPos) // Yes
                {
                    transform.position = meteor.transform.position;
                }
                else // No
                {
                    // Calculate the new position.
                    Vector3 oldPos = transform.position;
                    Vector3 newPos = Vector3.MoveTowards(oldPos, meteor.transform.position, trackSpeed * Time.deltaTime);

                    // Apply the new position.
                    transform.position = newPos;

                    // If the position has been matched, track the exact position.
                    if(newPos == meteor.transform.position)
                    {
                        trackExactPos = true;
                        OnMeteorTargeted();
                    }
                }
            }
            else
            {
                // Don't track the exact position if there's no meteor.
                trackExactPos = false;
            }

        }

    }
}