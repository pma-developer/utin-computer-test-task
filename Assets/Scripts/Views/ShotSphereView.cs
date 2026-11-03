using UnityEngine;
using ScriptableObjects;

namespace Views
{
    public class ShotSphereView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameConfig _gameConfig;
        
        private bool _isGrowing = false;
        private float _currentSize = 0f;
        private Vector3 _initialScale;

        public bool IsGrowing => _isGrowing;

        private void Awake()
        {
            _initialScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void StartGrowing()
        {
            if (_currentSize >= _gameConfig.ShotMaxSize) return;
            
            _isGrowing = true;
            _currentSize = _gameConfig.ShotMinSize;
            transform.localScale = _initialScale * _currentSize;
        }

        public void StopGrowing()
        {
            if (!_isGrowing) return;
            
            _isGrowing = false;
            FireShot();
        }

        private void Update()
        {
            if (!_isGrowing) return;
            
            _currentSize += _gameConfig.ShotGrowthRate * Time.deltaTime;
            _currentSize = Mathf.Min(_currentSize, _gameConfig.ShotMaxSize);
            
            transform.localScale = _initialScale * _currentSize;
        }

        private void FireShot()
        {
            // TODO: Implement shot firing logic
            // For now, just reset the sphere
            transform.localScale = Vector3.zero;
            _currentSize = 0f;
        }
    }
}
