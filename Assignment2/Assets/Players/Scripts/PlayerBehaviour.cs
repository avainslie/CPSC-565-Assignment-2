using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players{
    public class PlayerBehaviour : MonoBehaviour
    {
        // Can be set by user
        [SerializeField] private int seed;

        [SerializeField] private Vector3 snitchPos; // for debugging purposes
        [SerializeField] private float exhaustion;
        [SerializeField] private bool unconscious;
        [SerializeField] private float weight;
        [SerializeField] private float maxVelocity;
        [SerializeField] private float aggressiveness;
        [SerializeField] private float maxExhaustion;

        // Scriptable object holding settings for the player this PlayerBehaviour script is on
        public PlayerSettingsScriptable player;

        // Scriptable object holding settings for other players this player may bump into
        public PlayerSettingsScriptable otherPlayerScriptable;

        // Reference to the PlayerBehaviour script on other players this player may bump into
        private PlayerBehaviour otherPlayerScript;

        // Reference to another player this player may bump into
        public GameObject otherPlayer;

        // Reference to the snitch
        public GameObject snitch;

        // 
        private Transform parentTransform;
        
        private Rigidbody rigidbody;
        private System.Random steadyR; 
        private System.Random changingR;

        void Awake(){

            // Set rigid body to the one on the player in the scene
            rigidbody = GetComponent<Rigidbody>();

            // For pivot point
            //parentTransform = this.transform.parent.transform;

            // transform look at

            steadyR = new System.Random(seed);  
            changingR = new System.Random();

            exhaustion = 0f;

            // https://answers.unity.com/questions/1141391/accessing-script-from-another-object.html
            otherPlayerScript = GetComponent<PlayerBehaviour>();

        }

        // Spawn team
        void Start(){
            weight = createPlayer(player.weight, player.weightStdDev);
            maxVelocity = createPlayer(player.maxVelocity, player.maxVelocityStdDev);
            aggressiveness = createPlayer(player.aggressiveness, player.aggressivenessStdDev);
            maxExhaustion = createPlayer(player.maxExhaustion, player.maxExhaustionStdDev);
        }

        // Update is called once per frame
        // Taken and modified from CPSC 565 - Lecture 8
        void Update()
        {
            snitchPos = snitch.transform.position;
            // Only apply force if player is not unconscious
            if (!unconscious){
                float dist = (Vector3.Distance(transform.position, snitch.transform.position)) / 10;
                Vector3 dir = (snitch.transform.position - transform.position);
                dir.Normalize();
                // Vector3 times a float
                rigidbody.AddForce(dir * dist);
                //parentTransform.rotation = Quaternion.LookRotation(dir);
            }
            // Stop moving if unconscious
            else if(unconscious){
                rigidbody.velocity = Vector3.zero;
            }
            
            //adjustExhaustion();

            
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
                //unconsciousTheMethod()
                Debug.Log("player from team "+player.team+" is unconscious!");
            }
        }


        // When two players collide
        private void OnTriggerEnter(Collider other){
            //Debug.Log("something has collided");
            
            // Players from the same team collide
            if (other.gameObject.CompareTag(player.team)){
                Debug.Log("players of same team collided");
                int val = (int)(steadyR.NextDouble()*100);
                // Player with the highest exhaustion will become unconscious 5% of the time
                if (exhaustion < otherPlayerScript.exhaustion){
                    if(val > 95){
                        //unconsciousTheMethod()
                    }
                }
                // Both have the same exhaustion, both become unconscious
                else if (exhaustion == otherPlayerScript.exhaustion){
                    //unconsciousTheMethod()
                }
                

            }
            // Players from opposing teams collide
            else if (other.gameObject.CompareTag(otherPlayerScriptable.team)){
                Debug.Log("diff team players collided");
                // Code from assignment description
                double player1Value = player.aggressiveness * (steadyR.NextDouble() * (1.2 - 0.8) + 0.8) 
                * (1 - (exhaustion / player.maxExhaustion));

                double player2Value = otherPlayerScriptable.aggressiveness * (steadyR.NextDouble() * 
                (1.2 - 0.8) + 0.8) * (1 - (exhaustion / otherPlayerScriptable.maxExhaustion));
                
                // Compare who had the lower value 
                if (player2Value > player1Value){
                    //unconsciousTheMethod();
                    Debug.Log("message from: " + player.team+ "....p2 value greater than p1. The winning team name: " 
                    + otherPlayerScriptable.team + " the losting team name: " + player.team);
                    Debug.Log(player.team + unconscious);
                }
                // Both players knocked out
                else if (player2Value == player1Value){
                    //unconsciousTheMethod();
                    Debug.Log("Ouch");
                }
            }

        }
        private void unconsciousTheMethod(){
            unconscious = true;
            rigidbody.useGravity = true;
        }

        // https://stackoverflow.com/questions/218060/random-gaussian-variables 
        private float createPlayer(float mean, float std){
            // Reset random for variability 
            changingR = new System.Random((int) System.DateTime.Now.Ticks);

           float u1 = (float) (1.0 - changingR.NextDouble());
           float u2 = (float) (1.0 - changingR.NextDouble());

           float randStdNormal = (float) (Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2));

           float randNormal = (float) (mean + std * randStdNormal);
           
           return randNormal;
        } 
    }      
}
