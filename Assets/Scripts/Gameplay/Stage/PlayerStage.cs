using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The player for the stage.
    public class PlayerStage : MonoBehaviour
    {
        // The stage manager.
        public StageManager stageManager;

        // The laser prefab.
        public LaserShot laserShotPrefab;

        // The player's active laser shot.
        public LaserShot laserShotActive;

        // The laser shot object pool.
        private List<LaserShot> laserShotPool = new List<LaserShot>();

        // If the laser shot pool should be used.
        private bool useLaserShotPool = true;

        // If 'true', the player can shoot multiple laser shots.
        private bool multipleLaserShots = false;

        // The number of points.
        private float points = 0;

        // If 'true', the player can be stunned.
        public bool stunPlayer = true;

        // The player stun timer.
        private float playerStunTimer = 0.0F;

        // Player stun timer max.
        public const float PLAYER_STUN_TIMER_MAX = 0.5F;

        // Start is called before the first frame update
        void Start()
        {
            // If the stage manger isn't set, set it.
            if (stageManager == null)
                stageManager = StageManager.Instance;

            // Set the player.
            if(stageManager.player == null)
                stageManager.player = this;
        }

        // Shoots the laser shot with the default colour.
        public LaserShot ShootLaserShot(float outputValue)
        {
            return ShootLaserShot(outputValue, Color.white);
        }

        // Shoots the laser shot and gives it the provided color.
        public LaserShot ShootLaserShot(float outputValue, Color color)
        {
            // If there are not multiple laser shots.
            if (!multipleLaserShots)
            {
                // The player already has an active laser shot.
                if (laserShotActive != null)
                {
                    // If the laser shot is active, return that shot.
                    if (laserShotActive.isActiveAndEnabled)
                    {
                        return laserShotActive;
                    }
                }
            }

            
            // // If the game is slowed down, return to normal speed.
            // if (stageManager.IsSlowSpeed())
            //     stageManager.SetToNormalSpeed();

            // The laser shot to be set.
            LaserShot newShot = null;

            // Checks if the laser shot pool should be used.
            if(useLaserShotPool)
            {
                // There's a laser shot in the pool.
                if(laserShotPool.Count > 0)
                {
                    // Get the shot at the front of the list, and remove it.
                    newShot = laserShotPool[0];
                    laserShotPool.RemoveAt(0);

                    // Turn the shot on.
                    newShot.gameObject.SetActive(true);

                }
                else // No laser shot in the pool.
                {
                    newShot = Instantiate(laserShotPrefab);
                }
            }
            else // Don't use the pool, so make a new object.
            {
                newShot = Instantiate(laserShotPrefab);
            }

            // Set the player for the laser shot.
            newShot.player = this;

            // Set the spawn point.
            stageManager.stage.SetLaserShotToSpawnPositionY(newShot);

            // Gets the meteor.
            Meteor meteor = stageManager.meteorTarget.GetMeteor();

            // If the meteor is not null, target it.
            if (meteor != null)
            {
                newShot.Shoot(meteor.gameObject);
            }
            else // No meteor, so no target.
            {
                newShot.Shoot(null);
            }


            // Sets the laser shot's output value, color, and sets it as the active laser shot.
            newShot.outputValue = outputValue;
            newShot.spriteRenderer.color = color;
            laserShotActive = newShot;

            // Returns the new shot.
            return newShot;
        }

        // Returns 'true' if the laser shot pool should be used.
        public bool UseLaserShotPool()
        {
            return useLaserShotPool;
        }

        // Returns the laser shot to the object pool.
        public void ReturnLaserShotToPool(LaserShot laserShot)
        {
            // If this is the active laser shot, remove it.
            if (laserShotActive == laserShot)
                laserShotActive = null;

            // Resets the laser shot.
            laserShot.ResetLaserShot();

            // Turn off the laser shot and return it to the pool.
            laserShot.gameObject.SetActive(false);
            laserShotPool.Add(laserShot);
        }

        // Gets the points.
        public float GetPoints()
        {
            return points;
        }
        
        // Set the player's points.
        public void SetPoints(float newPoints)
        {
            // Set value.
            points = newPoints;

            // The points can't be negative.
            if(points < 0)
                points = 0;

            // On points changed.
            OnPointsChanged();
        }

        // Gives points to the player.
        public void GivePoints(float pointsAdd)
        {
            SetPoints(points + pointsAdd);
        }

        // Calculates and gives the player points.
        public void CalculateAndGivePoints(Meteor meteor)
        {
            GivePoints(stageManager.CalculatePoints(meteor));
        }

        // Removes points from the player.
        public void RemovePoints(float pointsMinus)
        {
            SetPoints(points - pointsMinus);
        }

        // Called when the points have changed.
        public void OnPointsChanged()
        {
            // The player's points have changed.
            stageManager.OnPlayerPointsChanged();
        }

        // STUN
        // Is the player stunned?
        public bool IsPlayerStunned()
        {
            return playerStunTimer > 0.0F;
        }

        // Stuns the player.
        public virtual void StunPlayer()
        { 
            OnPlayerStunStarted();
        }

        // Unstuns the player.
        public virtual void UnstunPlayer()
        {
            OnPlayerStunEnded();
        }

        // The player has gotten stunned.
        protected virtual void OnPlayerStunStarted()
        {
            playerStunTimer = PLAYER_STUN_TIMER_MAX;
            stageManager.stageUI.MakeUnitButtonsUninteractable();
        }

        // The player is no longer stunned.
        protected virtual void OnPlayerStunEnded()
        {
            playerStunTimer = 0.0F;

            // The meteor's target is being targeted exactly, make the buttons interactable again.
            if(stageManager.meteorTarget.IsMeteorTargetedExactly())
            {
                stageManager.stageUI.MakeUnitButtonsInteractable();
            }            
        }

        // Update is called once per frame
        void Update()
        {
            // If the player stun timer is running, and the game is not paused.
            if(playerStunTimer > 0.0F && !stageManager.IsGamePaused())
            {
                // Reduce by unscaled delta time.
                playerStunTimer -= Time.unscaledDeltaTime;

                // No longer stunned.
                if(playerStunTimer <= 0)
                {
                    playerStunTimer = 0;
                    OnPlayerStunEnded();
                }
            }
        }
    }
}