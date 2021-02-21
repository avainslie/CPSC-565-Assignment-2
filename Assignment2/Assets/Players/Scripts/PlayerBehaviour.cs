using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace players{
    public class PlayerBehaviour : MonoBehaviour
    {

        // Can be set by user
        [SerializeField] private int seed;
        [SerializeField] private float maxVelocity;
        [SerializeField] private float minVelocity;
        [SerializeField] private int speed;

        private Rigidbody rigidbody;
        private Vector3 oldForceDir;
        private System.Random rng;
        private float maxHeight;
        private float minHeight;
        private float mass;

        void Awake(){

            rng = new System.Random(seed);  

            oldForceDir = Vector3.up * 5;

            mass = 85f;
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Make snitch move up first before moving randomly
            transform.Translate(oldForceDir);
            

            // Set rigid body to the one on the player in the scene
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 forceDir = new Vector3((float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble()*2) - 1);

            // Add the random Vector3 to the snitch's physics if it was not the last move the player made
            // Inspired by Omar's code 
            // https://github.com/omaddam/Boids-Simulation/blob/develop/Assets/Boids/Scripts/Bird.cs
            if (forceDir != oldForceDir){
            
                // Initialize the new velocity
                Vector3 acceleration = Vector3.zero;

                // Compute alignment
                // speed is the magnitude of the vector while forceDir is the direction
                acceleration += forceDir * speed;

                // Compute the new velocity, taking into account mass
                Vector3 velocity = rigidbody.velocity;
                velocity += (acceleration / mass) * Time.deltaTime;

                // Ensure the velocity remains within the accepted range
                velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude,
                    minVelocity, maxVelocity);

                // Apply velocity
                rigidbody.velocity = velocity;

                // Update rotation
                transform.forward = rigidbody.velocity.normalized;

                //rigidbody.AddForce(forceDir * 15);
                oldForceDir = forceDir;
            }  

            
        }
    }       
}

