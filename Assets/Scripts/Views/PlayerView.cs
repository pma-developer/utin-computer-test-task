using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Transform _pathwayToEnd;
        [Header("Animation Settings")]
        [SerializeField] private float _jumpHeight = 0.5f;
        [SerializeField] private float _jumpLength = 1.0f;
        
        public void SetSize(float size)
        {
            transform.localScale = Vector3.one * size;
            Vector3 prev = _pathwayToEnd.localScale;
            _pathwayToEnd.localScale = new Vector3(size, prev.y, prev.z);
        }

        public Sequence PlayMoveAnimation(Vector3 targetPosition, float duration = 1f)
        {
            float distance = Vector3.Distance(transform.position, targetPosition);
            
            if (distance <= 0.01f)
            {
                return DOTween.Sequence().Append(transform.DOMove(targetPosition, 0));
            }
            
            Vector3 startScale = transform.localScale;
            int numJumps = Mathf.Max(1, Mathf.RoundToInt(distance / _jumpLength));
            transform.DOKill();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                transform.DOJump(targetPosition, _jumpHeight, numJumps, duration)
                    .SetEase(Ease.Linear)
            );

            sequence.Join(
                transform.DOScaleY(startScale.y * 1.25f, duration / (numJumps * 2))
                    .SetLoops(numJumps * 2, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
            );

            return sequence;
        }
    }
}