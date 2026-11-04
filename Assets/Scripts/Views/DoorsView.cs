using System;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class DoorsView : MonoBehaviour
    {
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private Transform _rightDoor;

        private Quaternion _leftDoorInitialRotation;
        private Quaternion _rightDoorInitialRotation;

        private void Awake()
        {
            _leftDoorInitialRotation = _leftDoor.localRotation;
            _rightDoorInitialRotation = _rightDoor.localRotation;
        }

        public void PlayOpenAnimation(float duration = 0.5f)
        {
            _leftDoor.DOLocalRotate(new Vector3(0f, -270f, 0f), duration);
            _rightDoor.DOLocalRotate(new Vector3(0f, 90f, 0f), duration);
        }

        public void ResetDoors()
        {
            _leftDoor.DOKill();
            _rightDoor.DOKill();
            _leftDoor.localRotation = _leftDoorInitialRotation;
            _rightDoor.localRotation = _rightDoorInitialRotation;
        }
    }
}