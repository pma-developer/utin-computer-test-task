using UnityEngine;

namespace Views
{
    public class ShotSphereView : MonoBehaviour
    {
        private Vector3 _initialScale;
        private bool _isFiring = false;
        private Vector3 _fireDirection;
        private float _fireSpeed = 10f;

        public event System.Action<Vector3> ShotCollided;

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

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isFiring) return;

            ShotCollided?.Invoke(collision.contacts[0].point);
        }
    }
}
