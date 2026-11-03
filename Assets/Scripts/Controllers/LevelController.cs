using System;
using DG.Tweening;
using UnityEngine;
using Views;
using ScriptableObjects;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private ShotSphereView _shotSphereView;
        [SerializeField] private ExplosionView _explosionView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _shotSphereSpawnPoint;
        [SerializeField] private Transform _target;
        [SerializeField] private PathChecker _pathChecker;

        
        private bool _isGrowing = false;
        private float _currentShotSize = 0f;
        private float _currentPlayerSize = 1f;

        public event Action LevelFinished;
        public event Action LevelFailed;


        private void Start()
        {
            _shotSphereView.ShotCollided += OnShotCollided;
            _explosionView.ExplosionCollided += OnExplosionCollided;
            ResetShot();
            _explosionView.gameObject.SetActive(false);
            
            _currentPlayerSize = _gameConfig.PlayerInitialScale;
            _playerView.SetSize(_currentPlayerSize);
        }

        private void OnDestroy()
        {
            _shotSphereView.ShotCollided -= OnShotCollided;
            _explosionView.ExplosionCollided -= OnExplosionCollided;
        }

        public void OnShootPressed()
        {
            if (_currentShotSize >= _gameConfig.ShotMaxSize) return;
            
            _isGrowing = true;
            _currentShotSize = _gameConfig.ShotMinSize;
            _shotSphereView.SetSize(_currentShotSize);
        }

        public void OnShootReleased()
        {
            if (!_isGrowing) return;
            
            _isGrowing = false;
            
            if (_currentPlayerSize <= _gameConfig.PlayerMinScale)
            {
                LevelFailed?.Invoke();
                return;
            }
            
            Vector3 direction = (_target.position - _shotSphereView.transform.position).normalized;
            _shotSphereView.StartFiring(direction);
        }

        private void Update()
        {
            if (_isGrowing)
            {
                _currentShotSize += _gameConfig.ShotGrowthRate * Time.deltaTime;
                _currentShotSize = Mathf.Min(_currentShotSize, _gameConfig.ShotMaxSize);
                _shotSphereView.SetSize(_currentShotSize);

                _currentPlayerSize -= _gameConfig.PlayerShrinkRate * Time.deltaTime;
                _playerView.SetSize(_currentPlayerSize);
                if (_currentPlayerSize <= _gameConfig.PlayerMinScale)
                {
                    LevelFailed?.Invoke();
                }
            }
        }

        private void ResetShot()
        {
            _shotSphereView.SetSize(0f);
            _currentShotSize = 0f;
            _shotSphereView.SetPosition(_shotSphereSpawnPoint.position);
        }
        
        private void OnShotCollided()
        {
            float infectionRadius = _currentShotSize * _gameConfig.ShotExplosionRadiusMultiplier;
            
            _explosionView.gameObject.SetActive(true);
            _explosionView.transform.position = _shotSphereView.transform.position;
            ResetShot();
            _shotSphereView.StopFiring();
            
            _explosionView.PlayExplosion(infectionRadius).OnComplete(() => {
                Debug.Log($"ExplosionFinished");
                _explosionView.gameObject.SetActive(false);
            });
        }

        private void OnExplosionCollided(ObstacleView obstacle)
        {
            if (!obstacle.gameObject.activeSelf) return;
            
            obstacle.SetInfected(true);
            CheckPathClear();
        }

        private void CheckPathClear()
        {
            if (_pathChecker.IsPathClear(_playerView.transform.position, _target.position, _currentPlayerSize))
            {
                LevelFinished?.Invoke();
            }
        }
    }
}
