using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_MST
{
    // A script for special functions for the tutorial text box.
    public class TutorialTextBox : TextBox
    {

        // The speaker image.
        public Image speakerImage;

        // SPEAKER //
        // Shows the speaker image.
        public void ShowSpeakerImage()
        {
            speakerImage.gameObject.SetActive(true);
        }

        // Hides the speaker iamge.
        public void HideSpeakerImage()
        {
            speakerImage.gameObject.SetActive(false);
        }
    }
}