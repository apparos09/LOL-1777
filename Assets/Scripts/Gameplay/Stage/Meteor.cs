using System.Collections;
using System.Collections.Generic;
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

        // The conversion for the meteor.
        public UnitsInfo.UnitsConversion conversion;

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
            Laser laser;
            Barrier barrier;
            StageSurface surface;

            // Tries to grab relevant components.
            if (other.TryGetComponent(out laser)) // Laser
            {
                GivePoints();
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
       

        // Give points to the player.
        public void GivePoints()
        {
            // Give the player points.
            stageManager.player.GivePoints(stageManager.CalculatePoints(this));

            // Kill the meteor.
            Kill();
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
            stageManager.OnMeteorDestroyed(this);
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // Gets the velocity and clamps it.
            Vector2 velocity = rigidbody.velocity;
            velocity = Vector2.ClampMagnitude(velocity, stageManager.meteorSpeedMax);

            // Set the velocity.
            rigidbody.velocity = velocity;
        }


    }
}