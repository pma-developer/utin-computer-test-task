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
        [SerializeField] private ObstacleView _obstaclePrefab;
        [SerializeField] private Transform _spawnAreaMin;
        [SerializeField] private Transform _spawnAreaMax;
        [SerializeField] private Transform _obstaclesSpawnArea;
        [SerializeField] private Transform _playerSpawnPosition;

        
        private bool _isGrowing = false;
        private bool _isLevelFinished = false;
        private bool _isShotFiring = false;
        private float _currentShotSize = 0f;
        private float _currentPlayerSize = 1f;

        public event Action LevelWon;
        public event Action LevelFailed;


        private void Start()
        {
            GenerateObstacles();
            
            _shotSphereView.ShotCollided += OnShotCollidedWithObstacle;
            _shotSphereView.CollidedWithShotReset += OnShotCollidedWithResetTrigger;
            _explosionView.ExplosionCollided += OnExplosionCollided;
            
            _playerView.transform.position = _playerSpawnPosition.position;
            ResetShot();
            _explosionView.gameObject.SetActive(false);
            
            float calculatedInitialSize = CalculateRequiredPlayerSize();
            Debug.Log($"Calculated initial size: {calculatedInitialSize}");
            SetPlayerSize(calculatedInitialSize);
        }

        private void OnDestroy()
        {
            _shotSphereView.ShotCollided -= OnShotCollidedWithObstacle;
            _shotSphereView.CollidedWithShotReset -= OnShotCollidedWithResetTrigger;
            _explosionView.ExplosionCollided -= OnExplosionCollided;
        }

        private void GenerateObstacles()
        {
            for (int i = 0; i < _gameConfig.ObstacleCount; i++)
            {
                float randomX = UnityEngine.Random.Range(_spawnAreaMin.localPosition.x, _spawnAreaMax.localPosition.x);
                float randomZ = UnityEngine.Random.Range(_spawnAreaMin.localPosition.z, _spawnAreaMax.localPosition.z);
                Vector3 localSpawnPosition = new Vector3(randomX, _obstaclePrefab.transform.localPosition.y, randomZ);
                Vector3 worldSpawnPosition = _obstaclesSpawnArea.TransformPoint(localSpawnPosition);
                
                Instantiate(_obstaclePrefab, worldSpawnPosition, Quaternion.identity, _obstaclesSpawnArea);
            }
        }

        private void SetPlayerSize(float size)
        {
            _currentPlayerSize = size;
            _playerView.SetSize(_currentPlayerSize);
            if (_currentPlayerSize <= _gameConfig.PlayerMinScale)
            {
                _isLevelFinished = true;
                LevelFailed?.Invoke();
            }
        }

        private void SetShotSize(float size)
        {
            _currentShotSize = size;
            _shotSphereView.SetSize(_currentShotSize);
        }

        public void OnShootPressed()
        {
            if (_isLevelFinished) return;
            if (_isShotFiring) return;
            
            SetPlayerSize(_currentPlayerSize - _gameConfig.MinShotSizeCost);
            
            _isGrowing = true;
            SetShotSize(_gameConfig.ShotMinSize);
        }

        public void OnShootReleased()
        {
            if (_isLevelFinished) return;
            if (!_isGrowing) return;
            _isGrowing = false;
            _isShotFiring = true;
            
            Vector3 direction = (_target.position - _shotSphereView.transform.position).normalized;
            _shotSphereView.StartFiring(direction);
        }

        private void Update()
        {
            if (_isGrowing && !_isLevelFinished)
            {
                SetShotSize(_currentShotSize + _gameConfig.ShotGrowthRate * Time.deltaTime);
                SetPlayerSize(_currentPlayerSize - _gameConfig.PlayerShrinkRate * Time.deltaTime);
            }
        }

        private void ResetShot()
        {
            SetShotSize(0f);
            _shotSphereView.SetPosition(_shotSphereSpawnPoint.position);
            _shotSphereView.StopFiring();
            _isShotFiring = false;
        }


        private void OnShotCollidedWithResetTrigger()
        {
            ResetShot();
            WinIfPathIsClear();
        }

        private void OnShotCollidedWithObstacle()
        {
            float infectionRadius = _currentShotSize * _gameConfig.ShotExplosionRadiusMultiplier;
            
            _explosionView.gameObject.SetActive(true);
            _explosionView.transform.position = _shotSphereView.transform.position;
            _explosionView.PlayExplosion(_currentShotSize, infectionRadius).OnComplete(() => {
                _explosionView.gameObject.SetActive(false);
            });
            ResetShot();
            WinIfPathIsClear();
        }

        private void OnExplosionCollided(ObstacleView obstacle)
        {
            obstacle.SetInfected(true);
            WinIfPathIsClear();
        }

        private void WinIfPathIsClear()
        {
            if (_pathChecker.IsPathClear(_playerView.transform.position, _target.position, _currentPlayerSize))
            {
                _isLevelFinished = true;
                LevelWon?.Invoke();
            }
        }

        private float CalculateRequiredPlayerSize()
        {
            Vector3 playerStartPos = _playerView.transform.position;
            Vector3 targetPos = _target.position;
            
            RaycastHit[] allObstacles = _pathChecker.GetObstaclesAlongPath(playerStartPos, targetPos, 1f);
            
            float totalSizeConsumed = allObstacles.Length * _gameConfig.MinShotSizeCost;
            
            float minRequiredSize = totalSizeConsumed + _gameConfig.PlayerMinScale;
            
            float initialSize = minRequiredSize * _gameConfig.PlayerRequiredSizeMultiplier;

            return initialSize;
        }

        public void RestartLevel()
        {
            DestroyAllObstacles();
            GenerateObstacles();
            
            _playerView.transform.position = _playerSpawnPosition.position;
            _explosionView.gameObject.SetActive(false);
            
            float calculatedInitialSize = CalculateRequiredPlayerSize();
            SetPlayerSize(calculatedInitialSize);
            ResetShot();
            
            _isLevelFinished = false;
            _isGrowing = false;
            _isShotFiring = false;
        }

        private void DestroyAllObstacles()
        {
            for (int i = _obstaclesSpawnArea.childCount - 1; i >= 0; i--)
            {
                Transform child = _obstaclesSpawnArea.GetChild(i);
                if (child.GetComponent<ObstacleView>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
