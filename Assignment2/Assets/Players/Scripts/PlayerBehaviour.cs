using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players{
    public class PlayerBehaviour : MonoBehaviour
    {

        // Can be set by user
        [SerializeField] private int seed;
        [SerializeField] private float minVelocity;
        [SerializeField] private int speed;
        public GameObject otherObject;


        private Rigidbody rigidbody;
        private Vector3 oldForceDir;
        private System.Random rng;
        private float maxHeight;
        private float minHeight;
        public PlayerSettingsScriptable player;
        private float mass;
        private float maxVelocity;
        private float aggressiveness;
        private float maxExhaustion;
        
        void Awake(){

            rng = new System.Random(seed);  

            oldForceDir = Vector3.up * 5;

            mass = player.weight;
            maxVelocity = player.maxVelocity;
            aggressiveness = player.aggressiveness;
            maxExhaustion = player.maxExhaustion;

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

            // Taken from CPSC 565 - Lecture 8

            float dist = Vector3.Distance(transform.position, otherObject.transform.position);
            Vector3 dir = (otherObject.transform.position - transform.position);
            dir.Normalize();
            // Vector3 times a float
            rigidbody.AddForce(dir * dist);






            //Vector3 forceDir = new Vector3((float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble()*2) - 1);

            // Add the random Vector3 to the snitch's physics if it was not the last move the player made
            // Inspired by Omar's code 
            // https://github.com/omaddam/Boids-Simulation/blob/develop/Assets/Boids/Scripts/Bird.cs
            // if (forceDir != oldForceDir){
            
            //     // Initialize the new velocity
            //     Vector3 acceleration = Vector3.zero;

            //     // Compute alignment
            //     // speed is the magnitude of the vector while forceDir is the direction
            //     acceleration += forceDir * speed;

            //     // Compute the new velocity, taking into account mass
            //     Vector3 velocity = rigidbody.velocity;
            //     velocity += (acceleration / mass) * Time.deltaTime;

            //     // Ensure the velocity remains within the accepted range
            //     velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude,
            //         minVelocity, maxVelocity);

            //     // Apply velocity
            //     rigidbody.velocity = velocity;

            //     // Update rotation
            //     transform.forward = rigidbody.velocity.normalized;

            //     //rigidbody.AddForce(forceDir * 15);
            //     oldForceDir = forceDir;
                
            // }  

            
        }
    }       
}
