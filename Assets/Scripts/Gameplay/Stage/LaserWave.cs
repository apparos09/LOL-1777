using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // A wave that gets launched to knockback other meteors when the player gets a correct answer.
    public class LaserWave : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The player that this laser shot belongs to. 
        public PlayerStage player;

        // The collider for the meteor.
        public new Collider2D collider;

        // The rigidbody for the meteor.
        public new Rigidbody2D rigidbody;

        // The sprite renderer.
        public SpriteRenderer spriteRenderer;

        // The speed of the laser shot.
        public float maxSpeed = 100.0F;

        // The force for the laser shot when it hits the meteor. 
        public float meteorHitForce = 10.0F;

        // Gets set to 'true' when force should be applied.
        public bool applyForce = true;

        // Start is called before the first frame update
        void Start()
        {
            // If the stage manager is not set, set it.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // If the collider is not set, try to set it.
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // If the rigidbody is not set, try to set it.
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();

            // Apply the ignores for the physics bodies.
            ApplyPhysicsBodyIgnores();
        }

        // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // A meteor to be hit by the trigger.
            Meteor meteor;

            // Tries to get a meteor from the collision object.
            if(collision.gameObject.TryGetComponent(out meteor))
            {
                meteor.ApplyKnockbackForce(Vector2.up, meteorHitForce);
            }
        }

        // Launches the laser wave from the provided spawn point.
        public void Launch()
        {
            // Start applying force.
            applyForce = true;

            // Reset the velocity.
            ResetVelocity();
        }

        // Applies the ignore settings for the physics bodies.
        public void ApplyPhysicsBodyIgnores()
        {
            // Layer-based ignores are handled by the stage manager.

            // Does manual ignores just to be sure. These aren't really necessary, but they're here regardless.
            // Ignore collision with the stage surface.
            Physics2D.IgnoreCollision(collider, stageManager.stageSurface.collider, true);

            // Goes through all the barriers and ignores collision with them.
            foreach (Barrier barrier in stageManager.stageBarriers)
            {
                Physics2D.IgnoreCollision(collider, barrier.collider, true);
            }
        }

        // Clamps velocity using the provided max velocity.
        public void ClampVelocity(float maxVelocity)
        {
            // Clamp the velocity at the max speed.
            Vector2 velocity = rigidbody.velocity;
            velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
            rigidbody.velocity = velocity;
        }

        // Resets the laser shot's velocity.
        public void ResetVelocity()
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0;
        }

        // Kills the wave.
        public void Kill()
        {
            // TODO: implement object pool.
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // If force should be applied.
            if(applyForce)
            {
                // Generate the force and apply it.
                Vector2 force = Vector2.up * maxSpeed * Time.deltaTime;
                rigidbody.AddForce(force, ForceMode2D.Impulse);

                // Clamps the velocity.
                ClampVelocity(maxSpeed);
            }

            // If the laser isn't in the game area, destroy it.
            // This is used to stop the laser wave.
            if (!stageManager.stage.InGameArea(gameObject))
            {
                Kill();
            }
            else
            {
                // If the game area is not defined, kill the wave anyway.
                if(!stageManager.stage.IsGameAreaDefined())
                {
                    Debug.LogWarning("The game area is not defined, so the laser wave cannot be used.");
                    Kill();
                }
            }
        }
    }
}