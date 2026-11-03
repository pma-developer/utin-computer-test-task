using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class PlayerView : MonoBehaviour
    {
        
        [Header("Jump Settings")]
        [SerializeField] private float _jumpHeight = 0.5f;
        [SerializeField] private float _jumpLength = 1.0f;
        
        [Header("Size Settings")]
        [SerializeField] private float _minScale = 0.2f;
        [SerializeField] private float _initialScale = 1f;

        
        public void SetSize(float size)
        {
            transform.localScale = Vector3.one * size;
        }

        public Sequence PlayMoveAnimation(Vector3 targetPosition, float duration = 1f)
        {
            float distance = Vector3.Distance(transform.position, targetPosition);
            
            if (distance <= 0.01f)
            {
                return DOTween.Sequence().Append(transform.DOMove(targetPosition, 0));
            }

            int numJumps = Mathf.Max(1, Mathf.RoundToInt(distance / _jumpLength));
            transform.DOKill();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                transform.DOJump(targetPosition, _jumpHeight, numJumps, duration)
                    .SetEase(Ease.Linear)
            );

            sequence.Join(
                transform.DOScaleY(1.25f, duration / (numJumps * 2))
                    .SetLoops(numJumps * 2, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
            );

            return sequence;
        }
    }
}