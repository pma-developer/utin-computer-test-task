using UnityEngine;
using DG.Tweening;

namespace Views
{
    public class AnimatedTextView : MonoBehaviour
    {
        [SerializeField] private float _popDuration = 0.5f;
        [SerializeField] private float _hideDuration = 0.3f;
        [SerializeField] private float _idleScale = 1.05f;
        [SerializeField] private float _idleDuration = 0.8f;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            HideImmediate();
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }

        public void Show()
        {
            DOTween.Kill(this);

            transform.localScale = Vector3.zero;
            canvasGroup.alpha = 0f;

            transform.DOScale(1f, _popDuration).SetEase(Ease.OutBack);
            canvasGroup.DOFade(1f, _popDuration * 0.5f);

            transform.DOScale(_idleScale, _idleDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(_popDuration);
        }

        public void Hide()
        {
            DOTween.Kill(this);

            transform.DOScale(0f, _hideDuration).SetEase(Ease.InBack);
            canvasGroup.DOFade(0f, _hideDuration);
        }

        private void HideImmediate()
        {
            transform.localScale = Vector3.zero;
            canvasGroup.alpha = 0f;
        }
    }
}