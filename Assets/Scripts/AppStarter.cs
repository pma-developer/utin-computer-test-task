using System;
using DG.Tweening;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Views;


public class AppStarter : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private DoorsView _doorsView;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Transform _playerEndJumpTarget;
    private readonly CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        Application.targetFrameRate = 60;
        DOTween.Init();
        
        _playerView.PlayMoveAnimation(_playerEndJumpTarget.position, _gameConfig.EndJumpDuration).Play().OnComplete(WinTheGame);
    }

    void WinTheGame()
    {
        Debug.Log("Game Won");
    }

    void OnDestroy()
    {
        disposables.Dispose();
    }
}
