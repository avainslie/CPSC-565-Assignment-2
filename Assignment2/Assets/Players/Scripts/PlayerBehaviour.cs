using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Players{
    public class PlayerBehaviour : MonoBehaviour
    {
        #region Variables and fields
        

        // Can be set by user
        [SerializeField] private int seed;


        [SerializeField] private float velocity;
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

        // Reference to the snitch
        public GameObject snitch;

        public Score score;

        // 
        private Transform parentTransform;
        
        private Rigidbody rigidbody;
        private System.Random steadyR; 
        private System.Random changingR;

        #endregion

        #region Unity Methods

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
            snitchPos = snitch.transform.position; // FOR DEBUGGING

            // Only apply force if player is not unconscious
            if (!unconscious){

                float dist = (Vector3.Distance(transform.position, snitch.transform.position)) / 10;

                // Restrict top speed to MaxVelocity
                dist = Mathf.Clamp(dist, 0, maxVelocity);
                Vector3 dir = (snitch.transform.position - transform.position);
                dir.Normalize();
                Vector3 c = ComputeCollisionAvoidanceForce() * 15;

                rigidbody.velocity = (dir + c) * 5;
                transform.forward = rigidbody.velocity.normalized * Time.deltaTime;

                velocity = rigidbody.velocity.magnitude; // FOR DEBUGGING
                
                //parentTransform.rotation = Quaternion.LookRotation(dir);
            }            
            adjustExhaustion(velocity); 
            
        }
        #endregion

        #region Custom Methods

        // Increase exhaustion of player as they move and reduce exhaustion when appropriate 
        // Sources: 
        // https://answers.unity.com/questions/23992/current-speed-of-an-object.html
        // https://answers.unity.com/questions/341314/how-to-deplete-health-every-second.html 
        private void adjustExhaustion(float v){

            // If player is moving quickly make them more tired
            if (v <= maxVelocity && v > (maxVelocity / 2)){
                exhaustion += 1 * Time.deltaTime;
            }
            // Slower movement makes them less tired
            else{ exhaustion += 0.5f * Time.deltaTime; }

            if (exhaustion >= (maxExhaustion - 5)){
                StartCoroutine(rest());
            }
            
            // Check if exhaustion is at the max
            if (exhaustion >= maxExhaustion){
                StartCoroutine(unconsciousTheMethod());
            }
        }

        private IEnumerator rest(){
            rigidbody.velocity = Vector3.zero;
            yield return new WaitForSeconds(3);
            exhaustion = 0;
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
                        StartCoroutine(unconsciousTheMethod());
                    }
                }
                // Both have the same exhaustion, both become unconscious
                else if (exhaustion == otherPlayerScript.exhaustion){
                    StartCoroutine(unconsciousTheMethod());
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
                    StartCoroutine(unconsciousTheMethod());
                    Debug.Log("message from: " + player.team+ "....p2 value greater than p1. The winning team name: " 
                    + otherPlayerScriptable.team + " the losting team name: " + player.team);
                    Debug.Log(player.team + unconscious);
                }
                // Both players knocked out
                else if (player2Value == player1Value){
                    StartCoroutine(unconsciousTheMethod());
                    Debug.Log("Ouch");
                }
            }
            else if (player.team.Equals("Gryffindor") && other.gameObject.CompareTag("Snitch") && !score.gameOver){
                score.increaseScoreG();
            }
            else if (player.team.Equals("Slytherin") && other.gameObject.CompareTag("Snitch") && !score.gameOver){
                score.increaseScoreS();
            }

        }

        // When players are unconscious collide with ground and go back to start pos
        // https://stackoverflow.com/questions/30056471/how-to-make-the-script-wait-sleep-in-a-simple-way-in-unity 
        private IEnumerator unconsciousTheMethod(){
            
            part1();

            yield return new WaitForSeconds(10);
    
           part2();

        }

        private void part1(){
            unconscious = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.useGravity = true;
            if (tag.Equals("Gryffindor")){
                rigidbody.position = new Vector3(10, 1.3f, 0);
            }
            else if(tag.Equals("Slytherin")){
                rigidbody.position = new Vector3(-10, 1.3f, 0);
            }

        }

        private void part2(){
            unconscious = false;
            rigidbody.useGravity = false;
        }



        // https://stackoverflow.com/questions/218060/random-gaussian-variables 
        private float createPlayer(float mean, float std){
            // Reset random for variability 
            // https://answers.unity.com/questions/603000/generating-a-good-random-seed.html 
            changingR = new System.Random((int) System.DateTime.Now.Ticks);

           float u1 = (float) (1.0 - changingR.NextDouble());
           float u2 = (float) (1.0 - changingR.NextDouble());

           float randStdNormal = (float) (Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2));

           float randNormal = (float) (mean + std * randStdNormal);

           return randNormal;
        } 


        // Taken from CPSC 565 Lecture 10 - Omar's boid code 
        // https://github.com/omaddam/Boids-Simulation/blob/develop/Assets/Boids/Scripts/Bird.cs 
        private Vector3 ComputeCollisionAvoidanceForce()
        {
            // Check if heading to collision
            // "out" forces variable to be passed by reference
            if (!Physics.SphereCast(transform.position, 1, transform.forward, out RaycastHit hitInfo, 1, 1 << ~LayerMask.NameToLayer("Snitch"))){
                return Vector3.zero;
            }
            // Compute force
            else{
                return transform.position - hitInfo.point; 
            } 
        }

        #endregion
    }      
}
