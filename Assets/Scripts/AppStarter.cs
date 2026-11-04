using System;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Views;
using Controllers;


public class AppStarter : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private DoorsView _doorsView;
    [SerializeField] private LevelController _levelController;
    [SerializeField] private InputController _inputController;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private AnimatedTextView _winTextView;
    [SerializeField] private AnimatedTextView _lossTextView;
    [SerializeField] private Transform _playerEndJumpTarget;
    
    private bool _hasTriggeredDoors = false;
    
    void Start()
    {
        Application.targetFrameRate = 60;
        DOTween.Init();
        
        _inputController.ShootStarted += _levelController.OnShootPressed;
        _inputController.ShootCanceled += _levelController.OnShootReleased;
        
        _levelController.LevelWon += PlayFinishAnimation;
        _levelController.LevelFailed += OnLevelFailed;
    }


    private void CheckDoorDistance()
    {
        if (_hasTriggeredDoors) return;
        Vector3 playerPos = _playerView.transform.position;
        Vector3 doorPos = _doorsView.transform.position;
        playerPos.y = 0f;
        doorPos.y = 0f;
        if (Vector3.Distance(playerPos, doorPos) <= _gameConfig.DoorOpenDistance)
        {
            _hasTriggeredDoors = true;
            _doorsView.PlayOpenAnimation(_gameConfig.DoorOpenDuration);
        }
    }
    private void PlayFinishAnimation()
    {
        _playerView.PlayMoveAnimation(_playerEndJumpTarget.position, _gameConfig.EndJumpDuration)
            .OnUpdate(CheckDoorDistance)
            .OnComplete(() => _winTextView.Show());
    }

    private void OnLevelFailed()
    {
        _lossTextView.Show();
    }

    private void OnDestroy()
    {
        _inputController.ShootStarted -= _levelController.OnShootPressed;
        _inputController.ShootCanceled -= _levelController.OnShootReleased;
        _levelController.LevelWon -= PlayFinishAnimation;
        _levelController.LevelFailed -= OnLevelFailed;
    }
}
