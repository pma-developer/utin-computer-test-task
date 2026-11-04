using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization; // Required for DoTween

namespace Views
{
    public class ObstacleView : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _shrinkDuration = 0.5f;
        
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _infectedMaterial;
        
        private Renderer _renderer; 
        private Collider _collider; 

        private bool _isInfected = false;
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
        }

        public void SetInfected(bool infected)
        {
            _isInfected = infected;
            transform.DOKill(); 
            _collider.enabled = !infected;

            if (_isInfected)
            {
                _renderer.material = _infectedMaterial;
                transform.DOScale(0f, _shrinkDuration).SetEase(Ease.InBack)
                         .OnComplete(() => gameObject.SetActive(false));
            }
            else
            {
                gameObject.SetActive(true);
                transform.localScale = _originalScale;
                _renderer.material = _normalMaterial;
            }
        }
    }
}