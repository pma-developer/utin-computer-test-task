using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AppStarter : MonoBehaviour
{
    
    readonly CompositeDisposable disposables = new CompositeDisposable();

    void Start()
    {
        Application.targetFrameRate = 60;
        DOTween.Init();
    }

    void OnDestroy()
    {
        disposables.Dispose();
    }
}
