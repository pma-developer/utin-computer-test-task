using UnityEngine;
using Views;
using ScriptableObjects;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private ShotController _shotController;
        [SerializeField] private GameConfig _gameConfig;
        
        private ObstacleView[] _obstacles;

        private void Start()
        {
            _obstacles = FindObjectsOfType<ObstacleView>();
            _shotController.ShotCollided += OnShotCollided;
        }

        private void OnDestroy()
        {
            _shotController.ShotCollided -= OnShotCollided;
        }

        private void OnShotCollided(Vector3 collisionPoint)
        {
            float infectionRadius = _shotController.CurrentSize * _gameConfig.InfectionRadiusMultiplier;
            
            foreach (ObstacleView obstacle in _obstacles)
            {
                if (obstacle == null) continue;
                
                float distance = Vector3.Distance(collisionPoint, obstacle.transform.position);
                if (distance <= infectionRadius)
                {
                    obstacle.Infect();
                }
            }
        }
    }
}
