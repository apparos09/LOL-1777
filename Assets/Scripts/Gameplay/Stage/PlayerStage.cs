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
            OnPointsChanged();
        }

        // Removes points from the player.
        public void RemovePoints(float pointsMinus)
        {
            points -= pointsMinus;
            OnPointsChanged();
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

        }
    }
}