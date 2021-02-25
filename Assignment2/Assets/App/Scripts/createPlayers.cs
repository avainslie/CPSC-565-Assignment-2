using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Players{
    public class createPlayers : MonoBehaviour
    {
        public GameObject prefabG;
        public GameObject prefabS;
        public GameObject snitch;

        // Start is called before the first frame update
        // https://docs.unity3d.com/Manual/InstantiatingPrefabs.html 
        // void Start()
        // {
        //     float z = -10;

        //     // Instantiate players at a predetermined position and zero rotation.
        //     // Makes 6 players total
        //     for (int i = 0; i < 5; i ++){

        //         // Gryffindor players
        //         GameObject g = Instantiate(prefabG, new Vector3(10, 1.3f, z), Quaternion.identity);
        //         g.GetComponent<PlayerBehaviour>().snitch = this.snitch;


        //         // Slytherin players
        //         GameObject s = Instantiate(prefabS, new Vector3(-10, 1.3f, z), Quaternion.identity);
        //         s.GetComponent<PlayerBehaviour>().snitch = this.snitch;

        //         z += 3.5f;
            
        //     }
            
        // }
    }
}

