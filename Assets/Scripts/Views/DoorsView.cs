using System;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class DoorsView : MonoBehaviour
    {
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private Transform _rightDoor;

        public void Start()
        {
            PlayOpenAnimation();
        }

        public void PlayOpenAnimation(float duration = 0.5f)
        {
            _leftDoor.DOLocalRotate(new Vector3(0f, -270f, 0f), duration);
            _rightDoor.DOLocalRotate(new Vector3(0f, 90f, 0f), duration);
        }
    }
}