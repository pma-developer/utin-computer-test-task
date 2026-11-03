using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [Header("Animations")]
        [SerializeField] public float EndJumpDuration = 5f;
        [SerializeField] public float DoorOpenDistance = 4f;
        [SerializeField] public float DoorOpenDuration = 0.7f;
        
        [Header("Shot Settings")]
        [SerializeField] public float ShotGrowthRate = 0.5f;
        [SerializeField] public float ShotMaxSize = 2f;
        [SerializeField] public float ShotMinSize = 0.3f;
        [SerializeField] public float PlayerShrinkRate = 0.3f;
        [SerializeField] public float PlayerMinScale = 0.2f;
        [SerializeField] public float PlayerInitialScale = 1f;
    }
}