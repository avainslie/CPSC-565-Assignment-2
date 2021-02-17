using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GryffindorSettings", menuName = "ScriptableObjects/GryffindorSettingsScriptable", order = 1)]
    public class GryffindorSettingsScriptable : ScriptableObject {

        [Header("Player specific settings")]

        [Tooltip("The weight of the players. Affects momentum")]
        [SerializeField] private int weight = 85;

        [Tooltip("The max speed the players can obtain.")]
        [SerializeField] private float maxVelocity;

        [Tooltip("How forceful players are. Affects if they become unconscious upon collision with another player.")]
        [SerializeField] private int aggressiveness;

        [Tooltip("How tired players can become before needing to rest.")]
        [SerializeField] private int maxExhaustion;
        
    }

    
}
