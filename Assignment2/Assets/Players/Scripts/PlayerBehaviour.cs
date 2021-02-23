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
        private float exhaustion;


        public GameObject snitch;

        public PlayerSettingsScriptable otherPlayerScriptable;
        
        void Awake(){

            // Set rigid body to the one on the player in the scene
            rigidbody = GetComponent<Rigidbody>();

            rng = new System.Random(seed);  

            oldForceDir = Vector3.up * 5;

            mass = player.weight;
            maxVelocity = player.maxVelocity;
            aggressiveness = player.aggressiveness;
            maxExhaustion = player.maxExhaustion;
            exhaustion = 0f;

        }

        // Update is called once per frame
        void Update()
        {
            // Taken from CPSC 565 - Lecture 8

            float dist = (Vector3.Distance(transform.position, snitch.transform.position)) / 10;
            Vector3 dir = (snitch.transform.position - transform.position);
            dir.Normalize();
            // Vector3 times a float
            rigidbody.AddForce(dir * dist);

            
        }


        // Increase exhaustion of player as they move and reduce exhaustion when appropriate 
        private void adjustExhaustion(){

        }


        // When two players collide
        private void OnTriggerEnter(Collider other){
            Debug.Log("something has collided");
            
            if (other.gameObject.CompareTag(player.team)){
                //0.05 chance to result in one person unconscious
                Debug.Log("players of same team collided");
            }
            else if (other.gameObject.CompareTag(otherPlayerScriptable.team)){
                Debug.Log("diff team players collided");

                double player1Value = player.aggressiveness * (rng.NextDouble() * (1.2 - 0.8) + 0.8) 
                * (1 - (exhaustion / player.maxExhaustion));

                double player2Value = otherPlayerScriptable.aggressiveness * (rng.NextDouble() * 
                (1.2 - 0.8) + 0.8) * (1 - (exhaustion / otherPlayerScriptable.maxExhaustion));

                if (player1Value > player2Value){
                //otherObject == unconscious
                Debug.Log("message from: " + player.team+ "...p1 value greater than p2. The winning team name: " 
                + player.team + " the losting team name: " + otherPlayerScriptable.team);
                }
                else if (player2Value > player1Value){
                    //unconscious == true
                    Debug.Log("message from: " + player.team+ "....p2 value greater than p1. The winning team name: " 
                    + otherPlayerScriptable.team + " the losting team name: " + player.team);
                }
                else{
                    //both unconscious
                    Debug.Log("nobody wins");
                }
            }

        }


    }       
}
