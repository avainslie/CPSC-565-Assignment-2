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

        private bool unconscious;

        public GameObject snitch;

        public PlayerSettingsScriptable otherPlayerScriptable;

        public GameObject otherPlayer;
        private PlayerBehaviour otherPlayerScript;
        
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

            // https://answers.unity.com/questions/1141391/accessing-script-from-another-object.html
            otherPlayerScript = GetComponent<PlayerBehaviour>();

        }

        // Update is called once per frame
        // Taken and modified from CPSC 565 - Lecture 8
        void Update()
        {
            // Only apply force if player is not unconscious
            if (!unconscious){
                float dist = (Vector3.Distance(transform.position, snitch.transform.position)) / 10;
                Vector3 dir = (snitch.transform.position - transform.position);
                dir.Normalize();
                // Vector3 times a float
                rigidbody.AddForce(dir * dist);
            }
            // Stop moving if unconscious
            //else if(unconscious){
                //rigidbody.velocity = Vector3.zero;
            //}
            
            adjustExhaustion();

            
        }


        // Increase exhaustion of player as they move and reduce exhaustion when appropriate 
        // Sources: 
        // https://answers.unity.com/questions/23992/current-speed-of-an-object.html
        // https://answers.unity.com/questions/341314/how-to-deplete-health-every-second.html 
        private void adjustExhaustion(){

            // Current velocity in Vector3 format and then float
            Vector3 velo = rigidbody.velocity;
            float magVelo = velo.magnitude;

            // If player is moving quickly make them more tired
            if (magVelo <= player.maxVelocity && magVelo > (player.maxVelocity / 2)){
                exhaustion += 1 * Time.deltaTime;
                
                //Debug.Log("exhaustion at higher speed is : "+exhaustion);
            }
            // Slower movement makes them less tired
            else{
                exhaustion += 0.5f * Time.deltaTime;
                
                //Debug.Log("exhaustion at lower speed is : "+exhaustion);
            }
            
            // Check if exhaustion is at the max
            if (exhaustion >= player.maxExhaustion){
                unconscious = true;
                Debug.Log("player from team "+player.team+" is unconscious!");
            }
        }


        // When two players collide
        private void OnTriggerEnter(Collider other){
            Debug.Log("something has collided");
            
            // Players from the same team collide
            if (other.gameObject.CompareTag(player.team)){
                Debug.Log("players of same team collided");
                int val = (int)(rng.NextDouble()*100);
                // Player with the highest exhaustion will become unconscious 5% of the time
                if (exhaustion < otherPlayerScript.exhaustion){
                    if(val > 95){
                        unconscious = true;
                    }
                }
                // Both have the same exhaustion, both become unconscious
                else if (exhaustion == otherPlayerScript.exhaustion){
                    unconscious = true;
                }
                

            }
            // Players from opposing teams collide
            else if (other.gameObject.CompareTag(otherPlayerScriptable.team)){
                Debug.Log("diff team players collided");
                // Code from assignment description
                double player1Value = player.aggressiveness * (rng.NextDouble() * (1.2 - 0.8) + 0.8) 
                * (1 - (exhaustion / player.maxExhaustion));

                double player2Value = otherPlayerScriptable.aggressiveness * (rng.NextDouble() * 
                (1.2 - 0.8) + 0.8) * (1 - (exhaustion / otherPlayerScriptable.maxExhaustion));
                
                // Compare who had the lower value 
                if (player2Value > player1Value){
                    unconsciousTheMethod();
                    Debug.Log("message from: " + player.team+ "....p2 value greater than p1. The winning team name: " 
                    + otherPlayerScriptable.team + " the losting team name: " + player.team);
                    Debug.Log(player.team + unconscious);
                }
                // Both players knocked out
                else if (player2Value == player1Value){
                    unconsciousTheMethod();
                    Debug.Log("Ouch");
                }
            }

        }
        private void unconsciousTheMethod(){
            unconscious = true;
            rigidbody.useGravity = true;
        }

        // Create
        private void createPlayer(){
           float u1 = (float) (1.0 - rng.NextDouble());
           float u2 = (float) (1.0 - rng.NextDouble());

           float randStdNormal = (float) (Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2));

           float randNormal = (float) (player.weight + player.weightStdDev * randStdNormal);
        } 
    }      
}
