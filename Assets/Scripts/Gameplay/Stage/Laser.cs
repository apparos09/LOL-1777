using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_MST
{
    // The laser shot by the player.
    public class Laser : MonoBehaviour
    {
        // TODO: set as trigger collision.
        // The collider for the meteor.
        public new Collider2D collider;

        // The rigidbody for the meteor.
        public new Rigidbody2D rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            // If the collider is not set, try to set it.
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // If the rigidbody is not set, try to set it.
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}