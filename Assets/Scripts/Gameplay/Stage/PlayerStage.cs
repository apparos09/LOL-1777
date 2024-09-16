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

        // The number of points.
        public float points = 0;

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
        
        // Gives points to the player.
        public void GivePoints(float pointsAdd)
        {
            points += pointsAdd;
            OnPointsChange();
        }

        // Removes points from the player.
        public void RemovePoints(float pointsMinus)
        {
            points -= pointsMinus;
            OnPointsChange();
        }

        // Called when the points have changed.
        public void OnPointsChange()
        {
            // If the points goal has been reached, trigger the stage win.
            if(stageManager.IsPointsGoalReached(points))
            {
                stageManager.OnStageWon();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}