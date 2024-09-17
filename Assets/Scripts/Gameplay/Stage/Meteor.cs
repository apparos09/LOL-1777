using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace RM_MST
{
    // The meteor being spawned.
    public class Meteor : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The collider for the meteor.
        public new Collider2D collider;

        // The rigidbody for the meteor.
        public new Rigidbody2D rigidbody;

        // The meteor's spawn point.
        public Vector3 spawnPoint = new Vector3();

        // TODO: generate random values to display as options.
        // The conversion for the meteor.
        public UnitsInfo.UnitsConversion conversion;

        // Gets set to 'true' when the meteor is suffering from knockback.
        private bool inKnockback = false;

        // Start is called before the first frame update
        void Start()
        {
            // If the stage manager is not set, set it.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // If the collider is not set, try to set it.
            if(collider == null)
                collider = GetComponent<Collider2D>();

            // If the rigidbody is not set, try to set it.
            if(rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();
        }

        // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another collider2D/rigidbody2D.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEvent(collision.gameObject);
        }

        // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D only)
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnCollisionEvent(collision.gameObject);
        }

        // Called on a collision event.
        private void OnCollisionEvent(GameObject other)
        {
            // Possible collision objects.
            LaserShot laserShot;
            Barrier barrier;
            StageSurface surface;

            // Tries to grab relevant components.
            if (other.TryGetComponent(out laserShot)) // Laser
            {
                TryGivePoints(laserShot);
            }
            else if (other.TryGetComponent(out barrier)) // Barrier
            {
                ApplyDamageToBarrier(barrier);
            }
            else if (other.transform.TryGetComponent(out surface)) // Surface
            {
                ApplyDamageToStageSurface(surface);
            }
        }

        // Called when the meteor has been spawned.
        public void OnSpawn()
        {
            ResetVelocity();
            SetMeteorToSpawnPoint();
        }

        // Sets the meteor to its spawn point.
        public void SetMeteorToSpawnPoint()
        {
            transform.position = spawnPoint;
        }

        // Resets the meteor's velocity.
        public void ResetVelocity()
        {
            rigidbody.velocity = Vector2.zero;
        }

        // Gets the converted value for the meteor.
        public float GetConvertedValue()
        {
            // Checks if conversion is set to get the value.
            if(conversion != null)
            {
                return conversion.GetConvertedValue();
            }
            else
            {
                return -1;
            }
        }

        // Give points to the player.
        public bool TryGivePoints(LaserShot laserShot)
        {
            // Gets set based on if the laser shot's output value is correct.
            bool success;

            // If the values match, the laser shot was a success.
            if(laserShot.outputValue == conversion.GetConvertedValue())
            {
                success = true;
            }
            else // If the values don't match, the laser shot was a failure.
            {
                success = false;
            }
            // Give the player points.

            // Knock back the meteor and kill the laser.
            Vector3 forceDirec = laserShot.moveDirec;

            // If the force direction is 0, set it to the forward of the laser shot.
            if (forceDirec == Vector3.zero)
                forceDirec = laserShot.transform.forward;

            // Add force for knockback.
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(forceDirec.normalized * laserShot.meteorHitForce, ForceMode2D.Impulse);
            inKnockback = true;

            // If the laser shot was a success, kill the meteor.
            if(success)
            {
                stageManager.IncreaseCombo(); // Increase the combo.
                stageManager.player.CalculateAndGivePoints(this); // Give the player points.                                                                  // Kill the laser shot.
                laserShot.Kill(success); // Kill the laser.

                // Play the combo animation if the player has more than one in the combo count.
                if (stageManager.combo > 1)
                {
                    // Play the animation at the meteor's position.
                    stageManager.comboDisplay.PlayComboAnimationAtPosition(transform.position);
                }

                Kill(); // Kill the meteor.
            }
            else
            {
                // The meteor has survived.
                stageManager.ResetCombo(false); // Reset the combo.
                laserShot.Kill(success); // Kill the laser.
                stageManager.OnMeteorSurivived(this);
            }
            

            // Returns the success value.
            return success;
        }

        // Damage the barrier.
        public void ApplyDamageToBarrier(Barrier barrier)
        {
            barrier.ApplyDamage(1.0F);
            Kill();
        }

        // Damage the surface.
        public void ApplyDamageToStageSurface(StageSurface surface)
        {
            // TODO: implement.
            surface.ApplyDamage(1.0F);
            Kill();
        }

        // Kills the meteor.
        public void Kill()
        {
            stageManager.OnMeteorKilled(this);
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // If the meteor is moving downwards, cap the velocity.
            if(rigidbody.velocity.y < 0)
            {
                // The meteor was experiencing knockback.
                if(inKnockback)
                {
                    // Look for a target again to see if another meteor has gotten closer.
                    stageManager.meteorTarget.RemoveTarget();
                    inKnockback = false;
                }

                // Gets the velocity and clamps it.
                Vector2 velocity = rigidbody.velocity;
                velocity = Vector2.ClampMagnitude(velocity, stageManager.GetModifiedMeteorSpeedMax());

                // Set the velocity.
                rigidbody.velocity = velocity;
            }

            // Not in game area, so kill it.
            if(!stageManager.stage.InGameArea(gameObject))
            {
                Kill();
            }
        }


    }
}