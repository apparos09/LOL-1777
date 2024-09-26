using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // A character icon script.
    public class CharacterIcon : MonoBehaviour
    {
        // Character icon animation enum.
        public enum charIconAnim { none, neutral, happy, sad, angry, shocked }

        // The aniamtor.
        public Animator animator;

        [Header("Animations")]

        public string neutralAnim = "";
        public string happyAnim = "";
        public string sadAnim = "";
        public string angryAnim = "";
        public string shockedAnim = "";

        // Start is called before the first frame update
        void Start()
        {
            // If the animator isn't set, try getting it.
            if(animator == null)
                animator = GetComponent<Animator>();
        }

        // Called when an animation is started.
        public virtual void OnAnimationStart()
        {
            // ...
        }

        // Called when an animation finished.
        public virtual void OnAnimationEnd()
        {
            // ...
        }


        // ANIMATIONS
        // Play animation by type.
        public void PlayAnimation(charIconAnim anim)
        {
            switch(anim)
            {
                case charIconAnim.neutral:
                    PlayNeutralAnimation();
                    break;

                case charIconAnim.happy:
                    PlayHappyAnimation();
                    break;

                case charIconAnim.sad:
                    PlaySadAnimation();
                    break;

                case charIconAnim.angry:
                    PlayAngryAnimation();
                    break;
                
                case charIconAnim.shocked:
                    PlayShockedAnimation();
                    break;

            }
        }

        // Plays the neutral animation.
        public void PlayNeutralAnimation()
        {
            animator.Play(neutralAnim);
        }

        // Plays the happy animation.
        public void PlayHappyAnimation()
        {
            animator.Play(happyAnim);
        }

        // Plays the sad animation.
        public void PlaySadAnimation()
        {
            animator.Play(sadAnim);
        }

        // Plays the angry animation.
        public void PlayAngryAnimation()
        {
            animator.Play(angryAnim);
        }

        // Plays the shocked animation.
        public void PlayShockedAnimation()
        {
            animator.Play(shockedAnim);
        }

    }
}