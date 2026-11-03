using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] public float EndJumpDuration = 5f;
    }
}