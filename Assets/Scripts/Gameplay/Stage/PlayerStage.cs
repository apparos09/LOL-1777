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

        // The number of points.
        private float points = 0;

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

        // Shoots the laser shot.
        public LaserShot ShootLaserShot(float outputValue)
        {
            // Generates the laser shot, sets the spawn point, and shoots it.
            LaserShot newShot = Instantiate(laserShotPrefab);
            stageManager.stage.SetLaserShotToSpawnPositionY(newShot);

            // If the meteor is not null, target it.
            if(stageManager.meteorTarget.meteor != null)
            {
                newShot.Shoot(stageManager.meteorTarget.meteor.gameObject);
            }
            else // No meteor, so no target.
            {
                newShot.Shoot(null);
            }
                    

            // Sets the laser shot's output value.
            newShot.outputValue = outputValue;

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

        // Update is called once per frame
        void Update()
        {
            // TODO: this is for testing purposes, remove when unneeded (game only uses mouse/touch controls).
            // Only allow the player to shoot when a meteor is targeted.
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(stageManager.meteorTarget.meteor != null)
                    ShootLaserShot(stageManager.meteorTarget.meteor.GetConvertedValue());
                else
                    ShootLaserShot(1);

            }
        }
    }
}