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

            // If the game is slowed down, return to normal speed.
            if (!stageManager.IsSlowSpeed())
                stageManager.SetToNormalSpeed();

            // Generates the laser shot, sets the spawn point, and shoots it.
            LaserShot newShot = Instantiate(laserShotPrefab);
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
            stageManager.stageUI.MakeUnitButtonsInteractable();
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