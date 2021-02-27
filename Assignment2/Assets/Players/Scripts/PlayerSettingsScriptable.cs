using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettingsScriptable", order = 1)]

    public class PlayerSettingsScriptable : ScriptableObject {

        [Header("Player specific settings")]

        [Tooltip("The team the player belongs to")]
        [SerializeField] public string team;

        [Tooltip("The weight of the players. Affects momentum")]
        [SerializeField] public float weight = 80f;

        [SerializeField] public float weightStdDev = 15f;

        [Tooltip("The max speed the players can obtain.")]
        [SerializeField] public float maxVelocity = 25f;

        [SerializeField] public float maxVelocityStdDev = 2f;

        [Tooltip("How aggressive players are. Affects if they become unconscious upon collision with another player.")]
        [SerializeField] public float aggressiveness = 25f;

        [SerializeField] public float aggressivenessStdDev = 5f;

        [Tooltip("How tired players can become before needing to rest.")]
        [SerializeField] public float maxExhaustion = 55f;

        [SerializeField] public float maxExhaustionStdDev = 14f;

        [Tooltip("How distracted players can be.")]
        [SerializeField] public float distracted = 15f;

        [SerializeField] public float distractedStdDev = 1f;

        [Tooltip("How lazy players can be.")]
        [SerializeField] public float laziness = 50f;

        [SerializeField] public float lazinessStdDev = 10f;
        
    }
}
