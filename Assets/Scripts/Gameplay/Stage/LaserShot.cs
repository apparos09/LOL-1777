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
        public float maxSpeed = 65.0F;

        // The movement direction.
        public Vector3 moveDirec = Vector3.up;

        // The force for the laser shot when it hits the meteor. 
        public float meteorHitForce = 10.0F;

        // Gets set to 'true' when force should be applied.
        public bool applyForce = true;

        // TODO: account for measurement type?
        // The output value for this laser shot.
        [Tooltip("The output value attached to this laser shot. If it matches that of the meteor's, it is correct.")]
        public float outputValue = 0;

        [Header("Animation")]

        // The animator.
        public Animator animator;

        // The launch animation.
        public string launchAnim = "Laser Shot - Launch Animation";

        // The idle animation.
        public string idleAnim = "Laser Shot - Idle Animation";

        // The death animation.
        public string deathAnim = "Laser Shot - Death Animation";

        // Sets if animations are being used.
        private bool useAnimations = true;

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

            // Sets if animations are being used.
            animator.enabled = useAnimations;       
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

            // Apply force to the object.
            applyForce = true;

            // Plays the launch animation.
            if(useAnimations)
            {
                PlayLaunchAnimation();
            }
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
        public virtual void Kill(bool success)
        {
            // Gets the player.
            PlayerStage player = stageManager.player;

            // Checks if the shot was a success.
            if(success)
            {
                // Play a happy animation.
                stageManager.stageUI.PlayPartnersAnimation(CharacterIcon.charIconAnim.happy);
                
            }
            else // Not a success.
            {
                // Play a sad aniamtion.
                stageManager.stageUI.PlayPartnersAnimation(CharacterIcon.charIconAnim.sad);

                // Stuns the player if they can be stunned.
                if (player.stunPlayer)
                    player.StunPlayer();
            }

            // If this is the player's active shot, remove it.
            if (player.laserShotActive == this)
                player.laserShotActive = null;

            // Kill the rigidbody velocity.
            rigidbody.velocity = Vector2.zero;
            
            // Checks if animations should be used.
            if(useAnimations)
            {
                PlayDeathAnimation();
            }
            else
            {
                OnDeath();
            }
        }

        // Called when the laser shot has died.
        protected virtual void OnDeath()
        {
            Destroy(gameObject);
        }

        // Resets the laser shot.
        public void ResetLaserShot()
        {
            transform.forward = Vector3.forward;
            moveDirec = Vector3.up;
            applyForce = true;
        }

        // ANIMATION

        // Launch Animation
        protected void PlayLaunchAnimation()
        {
            animator.Play(launchAnim);
        }

        // Launch animation start.
        protected void OnLaunchAnimationStart()
        {
            // ...
        }


        // Launch animation end.
        protected void OnLaunchAnimationEnd()
        {
            PlayIdleAnimation();
        }

        // Idle animation.
        protected void PlayIdleAnimation()
        {
            animator.Play(idleAnim);
        }

        // Death
        protected void PlayDeathAnimation()
        {
            animator.Play(deathAnim);
        }

        // Death Start
        public void OnDeathAnimationStart()
        {
            applyForce = false;
            rigidbody.velocity = Vector2.zero;
        }

        // Death End
        public void OnDeathAnimationEnd()
        {
            applyForce = true;
            OnDeath();
        }


        // Update is called once per frame
        void Update()
        {
            // If force should be applied.
            if(applyForce)
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
                if (!stageManager.stage.InGameArea(gameObject))
                {
                    Kill(false);
                }
            }
            
        }
    }
}