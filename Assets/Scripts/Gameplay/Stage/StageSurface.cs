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

            // Set health to max.
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
        public void OnSurfaceKilled()
        {
            // The game is over.
            stageManager.OnStageLost();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}