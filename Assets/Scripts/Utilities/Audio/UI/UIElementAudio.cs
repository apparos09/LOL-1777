using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // The user interface audio.
    public abstract class UIElementAudio : MonoBehaviour
    {
        // THe audio for the user inputs.
        public AudioSource audioSource;

        // The audio clip for the toggle.
        public AudioClip audioClip;

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            // Add to the onValueChanged function.
            AddOnValueChanged();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Add OnValueChanged Delegate
        public abstract void AddOnValueChanged();

        // Remove OnValueChanged Delegate
        public abstract void RemoveOnValueChanged();

        // NOTE: this can vary from element to element, so it cannot be inherited.
        // Called when the UI element is triggered.
        // protected abstract void OnValueChanged();

        // Script is destroyed.
        protected void OnDestroy()
        {
            RemoveOnValueChanged();
        }
    }
}