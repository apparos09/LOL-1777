using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The surface of the stage.
    public class StageSurface : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The collider for the meteor.
        public new Collider2D collider;

        // TODO: make private when not testing.
        // The health of the surface.
        public float health = 1.0F;

        // The maximum health of the surface.
        public float maxHealth = 1.0F;

        [Header("Sprites")]

        // Top
        public SpriteRenderer topLayerRenderer;

        // Middle
        public SpriteRenderer middleLayerRenderer;

        // Bottom
        public SpriteRenderer bottomLayerRenderer;

        [Header("Animation")]

        // The animator for the surface.
        public Animator animator;

        // The empty animation state.
        public string emptyAnim = "Empty State";

        // The damage animation.
        public string damageAnim = "Surface - Damage Animation";

        // Start is called before the first frame update
        void Start()
        {
            // Set the instance.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // If the collider is not set, try to set it.
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // Set health to max.
            SetHealthToMax();
        }

        // Returns 'true' if health is at max.
        public bool IsHealthAtMax()
        {
            return health >= maxHealth;
        }


        // Set the health to the max.
        public void SetHealthToMax()
        {
            health = maxHealth;
        }

        // Sets the health of the stage surface (0 - max health).
        public void SetHealth(float newHealth)
        {
            // Saves the old health.
            float oldHealth = health;

            // Sets the health, and calls the related functions.
            health = Mathf.Clamp(newHealth, 0, maxHealth);

            // Calls related functions.
            OnHealthChanged();

            // The surface has been damaged.
            if (health < oldHealth)
            {
                // Play the damage animation.
                PlayDamageAnimation();

                // The surface has been damaged.
                stageManager.OnSurfaceDamaged();
            }

            // Check for death.
            CheckDeath();
        }

        // Adds health to the barrier.
        public void AddHealth(float heal)
        {
            SetHealth(health + heal);
        }

        // Applies damage to the surface.
        public void ReduceHealth(float damage)
        {
            SetHealth(health - damage);
        }

        // Called when the surface's health has changed.
        public virtual void OnHealthChanged()
        {
            stageManager.stageUI.UpdateSurfaceHealthBar();
        }

        // Checks if the surface is dead.
        public bool CheckDeath()
        {
            // If the surface health is 0 or less, kill it.
            if (health <= 0)
            {
                health = 0;
                KillSurface();
                return true;
            }
            else
            {
                return false;
            }
        }

        // Kills the surface.
        public void KillSurface()
        {
            health = 0;
            OnSurfaceKilled();
        }

        // Called when the surface has been killed.
        protected void OnSurfaceKilled()
        {
            // TOOD: add animation.

            // The game is over.
            stageManager.OnStageLost();
        }

        // Restores the stage surface.
        public void RestoreSurface()
        {
            gameObject.SetActive(true); // TOOD: replace with animation.
            SetHealthToMax();
            stageManager.stageUI.UpdateSurfaceHealthBar();
        }

        // ANIMATIONS
        // Plays the damage animation.
        public void PlayDamageAnimation()
        {
            animator.Play(damageAnim);
        }

        // On the start of the damage animation.
        public void OnDamageAnimationStart()
        {
            // ...
        }

        // On the end of the damage animation.
        public void OnDamageAnimationEnd()
        {
            // Plays the empty animation.
            animator.Play(emptyAnim);
        }
    }

}