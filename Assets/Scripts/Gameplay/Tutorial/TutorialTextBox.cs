using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_MST
{
    // A script for special functions for the tutorial text box.
    public class TutorialTextBox : MonoBehaviour
    {
        // The text box.
        public TextBox textBox;

        // The speaker image.
        public Image speakerImage;


        // OPEN/CLOSE Text Box
        // If the text box is visible, return 'true'.
        public bool IsTextBoxVisible()
        {
            return textBox.IsVisible();
        }

        // Opens the text box.
        public void OpenTextBox()
        {
            textBox.Open();
        }

        // Closes the text box.
        public void CloseTextBox()
        {
            textBox.Close();
        }

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