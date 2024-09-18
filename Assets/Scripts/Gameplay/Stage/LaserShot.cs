using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The laser shot by the player.
    public class LaserShot : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // TODO: set as trigger collision.
        // The collider for the meteor.
        public new Collider2D collider;

        // The rigidbody for the meteor.
        public new Rigidbody2D rigidbody;

        // The speed of the laser shot.
        public float maxSpeed = 50.0F;

        // The movement direction.
        public Vector3 moveDirec = Vector3.up;

        // The force for the laser shot when it hits the meteor. 
        public float meteorHitForce = 10.0F;

        // TODO: account for measurement type?
        // The output value for this laser shot.
        [Tooltip("The output value attached to this laser shot. If it matches that of the meteor's, it is correct.")]
        public float outputValue = 0;

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


            // TODO: maybe ignore by layer instead?

            // Ignore collision with the stage surface.
            Physics2D.IgnoreCollision(collider, stageManager.stageSurface.collider, true);

            // Goes through all the barriers and ignores collision with them.
            foreach(Barrier barrier in stageManager.stageBarriers)
            {
                Physics2D.IgnoreCollision(collider, barrier.collider, true);
            }    
        }

        // Shoots the shot at the provided target.
        public void Shoot(Vector3 target)
        {
            // Set the starting position.
            Vector3 startPos = transform.position;
            startPos.x = target.x;
            transform.position = startPos;

            // Sets the moveDirec of the shot based on the target.
            if(target != transform.position)
            {
                // Gets the target in a 2D space.
                Vector3 target2D = target;
                target2D.z = transform.position.z;

                // Calculates the move direction.
                moveDirec = (target2D - gameObject.transform.position).normalized;
            }
            else // No target, so just go up.
            {
                moveDirec = Vector3.up;
            }
        
            // TODO: rotate in direction of movement.
        }

        // Shoots the shot at the provided game object.
        public void Shoot(GameObject target)
        {
            // If the target is valid, shoot at it.
            // If there is no target, just shoot up.
            if(target != null)
            {
                Shoot(target.transform.position);
            }
            else
            {
                // No target, so send the laser shot's position.
                Shoot(transform.position);
            }
        }

        // Kills the laser shot. If 'true', then the laser shot's hit was a success.
        public void Kill(bool success)
        {
            // Gets the player.
            PlayerStage player = stageManager.player;

            // The shot was not a success, so stun the player.
            if(!success)
            {
                // Stuns the player if they can be stunned.
                if (player.stunPlayer)
                    player.StunPlayer();
            }

            // If this is the player's active shot, remove it.
            if (player.laserShotActive == this)
                player.laserShotActive = null;

            Destroy(gameObject);
        }

        // Resets the laser shot.
        public void ResetLaserShot()
        {
            transform.forward = Vector3.forward;
            moveDirec = Vector3.up;
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: maybe don't include delta time?
            // Calculate the force.
            Vector2 force = new Vector2();
            force.x = moveDirec.normalized.x * maxSpeed * Time.deltaTime;
            force.y = moveDirec.normalized.y * maxSpeed * Time.deltaTime;

            // Adds the amount of force.
            rigidbody.AddForce(force, ForceMode2D.Impulse);

            // Clamp the velocity at the max speed.
            Vector2 velocity = rigidbody.velocity;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
            rigidbody.velocity = velocity;

            // If the laser isn't in the game area, destroy it.
            if(!stageManager.stage.InGameArea(gameObject))
            {
                Kill(false);
            }
        }
    }
}