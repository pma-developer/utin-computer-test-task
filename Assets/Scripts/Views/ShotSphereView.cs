using System;
using UnityEngine;

namespace Views
{
    public class ShotSphereView : MonoBehaviour
    {
        [SerializeField] private LayerMask _shotResetLayer;
        private Vector3 _initialScale;
        private bool _isFiring = false;
        private Vector3 _fireDirection;
        private float _fireSpeed = 10f;

        public event Action ShotCollided;
        public event Action CollidedWithShotReset;

        private void Awake()
        {
            _initialScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void SetSize(float size)
        {
            transform.localScale = _initialScale * size;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void StartFiring(Vector3 direction)
        {
            _isFiring = true;
            _fireDirection = direction.normalized;
        }

        public void StopFiring()
        {
            _isFiring = false;
        }

        private void Update()
        {
            if (_isFiring)
            {
                transform.position += _fireDirection * _fireSpeed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!_isFiring) return;

            if (collider.TryGetComponent(out ObstacleView _))
            {
                ShotCollided?.Invoke();
            }

            if (((int)_shotResetLayer & (1 << collider.gameObject.layer)) != 0)
            {
                CollidedWithShotReset?.Invoke();
            }
        }
    }
}
