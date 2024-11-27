using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // A script that manages screen effects for the stage.
    public class ScreenEffects : MonoBehaviour
    {
        // The stage UI.
        public StageUI stageUI;

        [Header("Animation")]
        // The animator for the stage screen.
        public Animator animator;

        // Used to reset animations ofr the screen effects.
        public string emptyAnim = "Empty State";

        // An animation to make the edges glow red.
        public string edgeGlowRedAnim = "Screen Effect - Edge Glow - Red Animation";

        // Start is called before the first frame update
        void Start()
        {
            // If the stage UI isn't set, set it.
            if (stageUI == null)
                stageUI = StageUI.Instance;

            // If the animator isn't set, set it.
            if(animator == null)
                animator = GetComponent<Animator>();
        }

        // Plays the empty state animation.
        public void PlayEmptyAnimation()
        {
            animator.Play(emptyAnim);
        }

        // Plays the edge glow animation in red.
        public void PlayEdgeGlowRedAnimation()
        {
            animator.Play(edgeGlowRedAnim);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}