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

        // Applies damage to the surface.
        public void ApplyDamage(float damage)
        {
            health -= damage;

            // Updates the surface health bar.
            stageManager.stageUI.UpdateSurfaceHealthBar();

            // If the surface health is 0 or less, kill it.
            if (health <= 0)
            {
                health = 0;
                KillSurface();
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

    }

}