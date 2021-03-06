﻿using System.Collections;
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
        private Vector3 forceDir;

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

            oldForceDir = Vector3.up * 5;

            // NextDouble returns random float between 0.0 and 1.0, do math to get values between -1.0 and 1.0
            // Taken from CPSC 565 - Lecture 8
            forceDir = (new Vector3((float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble()*2) - 1)) * 5;
        }

        // Start is called before the first frame update
        void Start()
        {   
            // Set rigid body to the one on the snitch in the scene
            rigidbody = GetComponent<Rigidbody>(); 

            rigidbody.mass = 10f;

            // Move up and then randomly at the start of the game
            rigidbody.AddForce(oldForceDir * 15, ForceMode.Force);
            rigidbody.AddForce(forceDir * 15, ForceMode.Force);
        }

    

        // Update is called once per frame
        void FixedUpdate()
        {   
            int val = (int)(rng.NextDouble()*100);
            // 8% chance to go a new random direction
            if (val > 92){
                forceDir = (new Vector3((float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble()*2) - 1)) * 5;
            }
            
            // ForceMode.Force takes into account mass of the snitch 
            rigidbody.AddForce(forceDir * 15, ForceMode.Force);
            // Clamp the speed of the snitch to a value set by user
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, speed);
        }
    }
}
