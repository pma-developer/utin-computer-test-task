using UnityEngine;
using Views;
using ScriptableObjects;

namespace Controllers
{
    public class ShotController : MonoBehaviour
    {
        [SerializeField] private ShotSphereView _shotSphereView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _target;
        [SerializeField] private GameConfig _gameConfig;
        
        private bool _isGrowing = false;
        private float _currentSize = 0f;

        public event System.Action<Vector3> ShotCollided;
        public bool IsGrowing => _isGrowing;
        public float CurrentSize => _currentSize;

        private void Start()
        {
            _shotSphereView.ShotCollided += OnShotCollided;
            _shotSphereView.SetPosition(_playerView.transform.position);
        }

        private void OnDestroy()
        {
            _shotSphereView.ShotCollided -= OnShotCollided;
        }

        public void StartGrowing()
        {
            if (_currentSize >= _gameConfig.ShotMaxSize) return;
            
            _isGrowing = true;
            _currentSize = _gameConfig.ShotMinSize;
            _shotSphereView.SetSize(_currentSize);
        }

        public void StopGrowing()
        {
            if (!_isGrowing) return;
            
            _isGrowing = false;
            FireShot();
        }

        private void Update()
        {
            if (_isGrowing)
            {
                _currentSize += _gameConfig.ShotGrowthRate * Time.deltaTime;
                _currentSize = Mathf.Min(_currentSize, _gameConfig.ShotMaxSize);
                _shotSphereView.SetSize(_currentSize);
                
                _playerView.Shrink(_gameConfig.PlayerShrinkRate * Time.deltaTime);
            }
        }

        private void FireShot()
        {
            if (_target == null)
            {
                ResetShot();
                return;
            }

            Vector3 direction = (_target.position - _shotSphereView.transform.position).normalized;
            _shotSphereView.StartFiring(direction);
        }

        private void OnShotCollided(Vector3 collisionPoint)
        {
            _shotSphereView.StopFiring();
            ShotCollided?.Invoke(collisionPoint);
            ResetShot();
        }

        private void ResetShot()
        {
            _shotSphereView.SetSize(0f);
            _currentSize = 0f;
            _shotSphereView.SetPosition(_playerView.transform.position);
        }
    }
}
