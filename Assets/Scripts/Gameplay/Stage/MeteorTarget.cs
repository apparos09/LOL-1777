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
        protected Meteor meteor;

        // The target's movement speed.
        [Tooltip("The movement speed for the target when it homes in on a meteor's position.")]
        public float trackSpeed = 40.0F;

        // If 'true', the exact meteor position is tracked.
        private bool trackExactPos = false;

        [Header("Animation")]

        // Animator
        public Animator animator;

        // Lock In Animation
        public string lockInAnim = "Target - Lock In Animation";

        // Lock Out Animation
        public string lockOutAnim = "Target - Lock Out Animation";

        [Header("Audio")]

        // The lock-in sound effect.
        public AudioClip lockInSfx;

        // The lock-out sound effect.
        public AudioClip lockOutSfx;


        // Start is called before the first frame update
        void Start()
        {
            // Set the instance.
            if (stageManager == null)
                stageManager = StageManager.Instance;
        }

        // Returns the meteor being targeted.
        public Meteor GetMeteor()
        {
            return meteor;
        }

        // Called when the meteor is targeted.
        private void OnMeteorTargeted()
        {
            // Meteor is set.
            if (meteor != null)
            {
                // Conversion is set.
                if(meteor.conversion != null)
                {
                    stageManager.stageUI.UpdateUnitsTable(meteor.conversion.group);
                }
            }

            // Updates the unit buttons with the provied meteor.
            stageManager.stageUI.UpdateConversionAndUnitsButtons(meteor);

            // Plays the animation.
            PlayLockInAnimation();
        }

        // Returns 'true' if any meteor is targeted.
        public bool IsMeteorTargeted()
        {
            return meteor != null;
        }

        // Retruns 'true' if the provided meteor is being targeted.
        // If the provided meteor is null, and the targeted meteor is null, it still returns true.
        public bool IsMeteorTargeted(Meteor meteor)
        {
            return this.meteor == meteor;
        }

        // Returns 'true' if the meteor's exact position is being targeted.
        // If 'false', the meteor is either not being targeted, or the target is not on the meteor's exact position.
        public bool IsMeteorTargetedExactly()
        {
            return meteor != null && trackExactPos;
        }

        // Sets the meteor.
        public void SetTarget(Meteor newMeteor)
        {
            meteor = newMeteor;
            trackExactPos = false;

            // Plays the lock out animation.
            PlayLockOutAnimation();
        }

        // Removes the target for the meteor.
        public void RemoveTarget()
        {
            // Remove the meteor, stop tracking the exactp position, and clear the buttons.
            meteor = null;
            trackExactPos = false;
            
            // End the unit button multiple revelas.
            stageManager.stageUI.ClearConversionAndUnitsButtons();
            stageManager.stageUI.EndUnitButtonMultipleReveals();

            // Plays teh animation.
            PlayLockOutAnimation();
        }

        // ANIMATION
        // Plays the lock in aniamtion.
        protected void PlayLockInAnimation()
        {
            animator.Play(lockInAnim);
        }

        // Plays the lock out aniamtion.
        protected void PlayLockOutAnimation()
        {
            animator.Play(lockOutAnim);
        }

        // AUDIO
        // Plays the lock in sound effect.
        public void PlayLockInSfx()
        {
            stageManager.stageAudio.PlaySoundEffectWorld(lockInSfx);
        }

        // Plays the lock out SFX.
        public void PlayLockOutSfx()
        {
            stageManager.stageAudio.PlaySoundEffectWorld(lockOutSfx);
        }

        // Update is called once per frame
        void Update()
        {
            // ...
        }

        // Late update is called every frame, if the Behaviour is enabled.
        private void LateUpdate()
        {
            // Moved here from main Update().
            // Meteor is set.
            if (meteor != null)
            {
                // Should exact position be tracked?
                if (trackExactPos) // Yes
                {
                    transform.position = meteor.transform.position;

                    // If the player is not stunned, make sure the buttons are interactable.
                    if (!stageManager.player.IsPlayerStunned() && meteor.IsAlive())
                    {
                        stageManager.stageUI.MakeUnitButtonsInteractable();
                    }
                }
                else // No
                {
                    // Calculate the new position.
                    Vector3 oldPos = transform.position;
                    Vector3 newPos = Vector3.MoveTowards(oldPos, meteor.transform.position, trackSpeed * Time.deltaTime);

                    // Apply the new position.
                    transform.position = newPos;

                    // If the position has been matched, track the exact position.
                    if (newPos == meteor.transform.position)
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

                // Keep the unit buttons disabled and cleared if nothing is targeted.
                stageManager.stageUI.MakeUnitButtonsUninteractable();
                stageManager.stageUI.ClearConversionAndUnitsButtons();
            }
        }

    }
}