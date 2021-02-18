using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace snitch
{
        public class SnitchBehaviour : MonoBehaviour
    {
        // Can be set by user
        [SerializeField] private int seed;
        [SerializeField] private float maxVelocity;
        [SerializeField] private float minVelocity;
        [SerializeField] private int speed;

        private Rigidbody rigidbody;
        private System.Random rng;
        private Vector3 oldForceDir;
        private bool liftOff;
        private float maxHeight;
        private float minHeight;

        /****** PHYSICS 101 NOTES ******
        
        force = mass * acceleration
        
        acceleration = (final velocity - initial velocity) / change in time

        momentum = mass * velocity 

        velocity = displacement / change in time

        *******************************/


        // Only called once (before Start())
        private void Awake()
        {   
            rng = new System.Random(seed);  
            // An idea for later
            //Random.seed = System.DateTime.Now.Millisecond;
            // https://answers.unity.com/questions/1606295/transformrotate-doesnt-work-inside-start-method.html 


            oldForceDir = Vector3.up * 5;
            liftOff = false;
        }

        // Start is called before the first frame update
        // TODO: make it so snitch gradually comes up, not just starts floating
        void Start()
        {

            // Make snitch move up first before moving randomly
            transform.Translate(oldForceDir);


            
            

            // Set rigid body to the one on the snitch in the scene
            rigidbody = GetComponent<Rigidbody>();

            

        }

    

        // Update is called once per frame
        void Update()
        {
            // https://answers.unity.com/questions/1606295/transformrotate-doesnt-work-inside-start-method.html
            // if (!liftOff){
            //     if (rng.NextDouble() < 0.5){ transform.Rotate(Vector3.right * Time.deltaTime * 400); }
            //     else{ transform.Rotate(Vector3.left * Time.deltaTime * 300); } 
                
            //     //rigidbody.AddForce(transform.TransformDirection(oldForceDir) * 20);
            //     liftOff = true;
            // }


            //// A little weird movement, but random and fairly quick (can be sped up...more jerky at faster speeds)

            // NextDouble returns random float between 0.0 and 1.0, do math to get values between -1.0 and 1.0
            // Taken from CPSC 565 - Lecture 8
            Vector3 forceDir = new Vector3((float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble()*2) - 1);

            // Add the random Vector3 to the snitch's physics if it was not the last move the snitch made
            // Inspired by Omar's code 
            // https://github.com/omaddam/Boids-Simulation/blob/develop/Assets/Boids/Scripts/Bird.cs
            if (forceDir != oldForceDir){
            
                // Initialize the new velocity
                Vector3 acceleration = Vector3.zero;

                // Compute alignment
                acceleration += forceDir * speed;

                // Compute the new velocity
                Vector3 velocity = rigidbody.velocity;
                velocity += acceleration * Time.deltaTime;

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
