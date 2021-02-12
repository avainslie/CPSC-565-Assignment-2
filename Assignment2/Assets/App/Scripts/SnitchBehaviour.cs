using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnitchBehaviour : MonoBehaviour
{

    private Rigidbody rigidbody;
    public int seed;

    private System.Random rng;

    private void Awake()
    {
        rng = new System.Random(seed);   
    }

    // Start is called before the first frame update
    void Start()
    {

        // Set rigid body to the one on the snitch in the scene
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // NextDouble returns random float between 0.0 and 1.0
        // Taken from CPSC 565 - Lecture 8
        Vector3 forceDir = new Vector3((float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble() * 2) - 1, (float)(rng.NextDouble()*2) - 1);

        // 
        rigidbody.AddForce(forceDir);
    }
}
