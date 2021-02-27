using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Players{
    public class PlayerBehaviour : MonoBehaviour
    {
        #region Variables and fields
        

        // Can be set by user
        [SerializeField] private Vector3 c;


        [SerializeField] private int seed;
        
        [SerializeField] private bool Distracted;

        [SerializeField] private float velocity;
        [SerializeField] private Vector3 snitchPos; // for debugging purposes
        [SerializeField] private float exhaustion;
        [SerializeField] private bool unconscious;
        [SerializeField] private float weight;
        [SerializeField] private float maxVelocity;
        [SerializeField] private float aggressiveness;
        [SerializeField] private float maxExhaustion;
        [SerializeField] private float distraction;
        [SerializeField] private float laziness;

        // Scriptable object holding settings for the player this PlayerBehaviour script is on
        public PlayerSettingsScriptable player;

        // Scriptable object holding settings for other players this player may bump into
        public PlayerSettingsScriptable otherPlayerScriptable;

        // Reference to the PlayerBehaviour script on other players this player may bump into
        private PlayerBehaviour otherPlayerScript;

        // Reference to the snitch
        public GameObject snitch;

        public Score score;
        
        private Rigidbody rigidbody;
        private System.Random steadyR; 
        private System.Random changingR;
        private bool gPoint;
        private bool sPoint;
        #endregion

        #region Unity Methods

        void Awake(){

            // Set rigid body to the one on the player in the scene
            rigidbody = GetComponent<Rigidbody>();

            steadyR = new System.Random(seed);  
            changingR = new System.Random();

            exhaustion = 0f;
            Distracted = false;
            gPoint = false;
            sPoint = false;

            // https://answers.unity.com/questions/1141391/accessing-script-from-another-object.html
            otherPlayerScript = GetComponent<PlayerBehaviour>();

        }

        // Set values
        void Start(){
            weight = createPlayer(player.weight, player.weightStdDev);
            maxVelocity = createPlayer(player.maxVelocity, player.maxVelocityStdDev);
            aggressiveness = createPlayer(player.aggressiveness, player.aggressivenessStdDev);
            maxExhaustion = createPlayer(player.maxExhaustion, player.maxExhaustionStdDev);
            distraction = createPlayer(player.distracted,player.distractedStdDev);
            laziness = createPlayer(player.laziness, player.lazinessStdDev);
        }

        // Update is called once per frame
        // Taken and modified from CPSC 565 - Lecture 8
        void FixedUpdate()
        {
            snitchPos = snitch.transform.position; // FOR DEBUGGING

            // Only apply force if player is not unconscious
            if (!unconscious){

                float dist = (Vector3.Distance(transform.position, snitch.transform.position)) / 10;

                dist = Mathf.Clamp(dist, 0, maxVelocity);

                Vector3 dir = snitch.transform.position - transform.position;
                
                c = ComputeCollisionAvoidanceForce();
                
                Vector3 forceToApplyToPlayer = dir + c;
                //Vector3 forceToApplyToPlayer = c;
                forceToApplyToPlayer.Normalize();
                
                if (!getDistracted()){
                    // if less lazy, multiply by a higher number
                    if (laziness > 50){
                        //rigidbody.velocity = forceToApplyToPlayer * dist * 5;
                        //transform.forward = rigidbody.velocity.normalized * Time.deltaTime;

                        rigidbody.AddForce(forceToApplyToPlayer *  6, ForceMode.Force);

                        // Ensure players velocity never exceeds it's maximum
                        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);
                    }
                    else{
                        //rigidbody.velocity = forceToApplyToPlayer * dist * 10;
                        //transform.forward = rigidbody.velocity.normalized * Time.deltaTime;

                        rigidbody.AddForce(forceToApplyToPlayer *  10, ForceMode.Force);

                        // Ensure players velocity never exceeds it's maximum
                        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);
                    }
                }
            }            
            adjustExhaustion(rigidbody.velocity.magnitude);    
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
            
            // Players from the same team collide
            if (other.gameObject.CompareTag(player.team)){
                int val = (int)(steadyR.NextDouble()*100);
                // Player with the highest exhaustion will become unconscious 5% of the time
                if (exhaustion < otherPlayerScript.exhaustion){
                    if(val > 95){
                        StartCoroutine(unconsciousTheMethod());
                    }
                }
                // Both have the same exhaustion, both become unconscious
                else if (exhaustion == otherPlayerScript.exhaustion || other.gameObject.CompareTag("Ground")){
                    StartCoroutine(unconsciousTheMethod());
                }
            }

            // Players from opposing teams collide
            else if (other.gameObject.CompareTag(otherPlayerScriptable.team)){

                // Code from assignment description
                double player1Value = player.aggressiveness * (steadyR.NextDouble() * (1.2 - 0.8) + 0.8) 
                * (1 - (exhaustion / player.maxExhaustion));

                double player2Value = otherPlayerScriptable.aggressiveness * (steadyR.NextDouble() * 
                (1.2 - 0.8) + 0.8) * (1 - (exhaustion / otherPlayerScriptable.maxExhaustion));
                
                // Compare who had the lower value 
                if (player2Value > player1Value){
                    StartCoroutine(unconsciousTheMethod());
                }
                // Both players knocked out
                else if (player2Value == player1Value){
                    StartCoroutine(unconsciousTheMethod());
                }
            }
            else if (player.team.Equals("Gryffindor") && other.gameObject.CompareTag("Snitch") && !score.gameOver && !gPoint){
                gPoint = true;
                sPoint = false;
                score.increaseScoreG(1);
            }
            else if (player.team.Equals("Slytherin") && other.gameObject.CompareTag("Snitch") && !score.gameOver && !sPoint){
                sPoint = true;
                gPoint = false;
                score.increaseScoreS(1);
            }
            // Successive catches are worth double points
            else if (player.team.Equals("Gryffindor") && other.gameObject.CompareTag("Snitch") && !score.gameOver && gPoint){
                sPoint = true;
                gPoint = false;
                score.increaseScoreG(2);
            }
            else if (player.team.Equals("Slytherin") && other.gameObject.CompareTag("Snitch") && !score.gameOver && sPoint){
                sPoint = true;
                gPoint = false;
                score.increaseScoreS(2);
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
                rigidbody.position = new Vector3(10, 1.3f, (float)(Random.Range(-10f,10f)));
            }
            else if(tag.Equals("Slytherin")){
                rigidbody.position = new Vector3(-10, 1.3f, (float)(Random.Range(-10f,10f)));
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
            // https://answers.unity.com/questions/1164722/raycast-ignore-layers-except.html 
            if (!Physics.SphereCast(transform.position, 2, transform.forward, out RaycastHit hitInfo, 2, 1 << ~LayerMask.NameToLayer("Snitch"))){
                return Vector3.zero;
            }
            // Compute force going away from object player is about to hit
            else{
                return (transform.position - hitInfo.point) * 20; 
            } 
        }

        private bool getDistracted(){

            Vector3 forceDir = new Vector3((float)(changingR.NextDouble() * 2) - 1, (float)(changingR.NextDouble() * 2) - 1, (float)(changingR.NextDouble()*2) - 1);
            int val = (int)(steadyR.NextDouble()*100);

            // More distracted players have a higher chance of going a random direction
            // Being very distracted is very rare
            if (distraction >= 18f && val > 95){
                rigidbody.velocity = forceDir * 15;
                transform.forward = rigidbody.velocity.normalized * Time.deltaTime;
                Distracted = true;
                return true;
            }
            // Chances are better to see this distracted player, who will occasionally go a weird direction
            else if (distraction >= 16f && distraction < 18f && val > 98){
                rigidbody.velocity = forceDir * 15;
                transform.forward = rigidbody.velocity.normalized * Time.deltaTime;
                Distracted = true;
                return true;
            }
            return false;
        }


        

        #endregion
    }      
}
