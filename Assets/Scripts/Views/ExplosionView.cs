using System;
using UnityEngine;
using DG.Tweening;

namespace Views
{
    public class ExplosionView : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _duration = 0.5f;
        private MeshRenderer _renderer;
        private Material _materialInstance;
        private Color _initialColor;
        public event Action<ObstacleView> ExplosionCollided; 

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            
            _materialInstance = _renderer.material;
            _initialColor = _materialInstance.color;
        }

        public Sequence PlayExplosion(float maxScale)
        {
            transform.localScale = Vector3.one;
            _materialInstance.color = _initialColor;
            Color endColor = _initialColor;
            endColor.a = 0f;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(maxScale, _duration).SetEase(Ease.OutQuad));
            sequence.Join(_materialInstance.DOColor(endColor, _duration).SetEase(Ease.InQuad));
            return sequence;
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log(collider.gameObject.name);
            if (collider.gameObject.TryGetComponent(out ObstacleView obstacle))
            {
                ExplosionCollided?.Invoke(obstacle);
            }
        }
        private void OnDestroy()
        {
            if (_materialInstance != null)
            {
                Destroy(_materialInstance);
            }
        }
    }
}