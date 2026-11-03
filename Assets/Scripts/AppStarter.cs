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
    [SerializeField] private ShotSphereView _shotSphereView;
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
        
        SetupInput();
        
        _playerView.PlayMoveAnimation(_playerEndJumpTarget.position, _gameConfig.EndJumpDuration)
            .OnUpdate(CheckDoorDistance)
            .OnComplete(WinTheGame);
    }

    private void SetupInput()
    {
        _inputController.ShootStarted += _shotSphereView.StartGrowing;
        _inputController.ShootCanceled += _shotSphereView.StopGrowing;
    }

    private void Update()
    {
        if (_shotSphereView.IsGrowing)
        {
            _playerView.Shrink(_gameConfig.PlayerShrinkRate * Time.deltaTime);
        }
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
    void WinTheGame()
    {
        Debug.Log("Game Won");
        _winTextView.Show();
    }

    private void OnDestroy()
    {
        _inputController.ShootStarted -= _shotSphereView.StartGrowing;
        _inputController.ShootCanceled -= _shotSphereView.StopGrowing;
    }
}
