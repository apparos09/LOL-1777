using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The barrier for the stage.
    public class Barrier : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The barrier's sprite.
        public SpriteRenderer spriteRenderer;

        // The collider for the meteor.
        public new Collider2D collider;

        // The rigidbody for the meteor.
        public new Rigidbody2D rigidbody;

        // The health of the surface.
        public float health = 1.0F;

        // The maximum health of the surface.
        public float maxHealth = 1.0F;

        // Start is called before the first frame update
        void Start()
        {
            // Set the instance.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // If the collider is not set, try to set it.
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // If the rigidbody is not set, try to set it.
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();

            // Set the health to max.
            SetHealthToMax();
        }

        // Set the health to the max.
        public void SetHealthToMax()
        {
            health = maxHealth;
        }

        // Applies damage to the surface.
        public void ApplyDamage(float damage)
        {
            health -= damage;

            // If the surface health is 0 or less, kill it.
            if(health <= 0)
            {
                health = 0;
                KillBarrier();
            }
        }

        // Kills the barrier.
        public void KillBarrier()
        {
            health = 0;
            OnBarrierKilled();
        }

        // On the barrier being killed.
        public void OnBarrierKilled()
        {
            gameObject.SetActive(false);
        }

        // Restores the barrier.
        public void RestoreBarrier()
        {
            gameObject.SetActive(true);
            SetHealthToMax();
        }
    }
}