using UnityEngine;

namespace Views
{
    public class ObstacleView : MonoBehaviour
    {
        [SerializeField] private float _infectionRadius = 2f;
        [SerializeField] private float _explosionDelay = 0.5f;

        private bool _isInfected = false;

        public float InfectionRadius => _infectionRadius;
        public bool IsInfected => _isInfected;

        public void Infect()
        {
            if (_isInfected) return;
            
            _isInfected = true;
            
            Invoke(nameof(Explode), _explosionDelay);
        }

        private void Explode()
        {
            // TODO: Add explosion effect
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _infectionRadius);
        }
    }
}
