using UnityEngine;

namespace Controllers
{
    public class PathChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleLayer;

        public bool IsPathClear(Vector3 start, Vector3 end, float playerSize)
        {
            float playerRadius = playerSize / 2;
            if (Physics.CheckSphere(start, playerRadius, _obstacleLayer))
            {
                Debug.Log("obstacle at the start");
                return false;
            }

            Vector3 offset = end - start;
            return !Physics.SphereCast(
                start,
                playerRadius,
                offset.normalized,
                out _,
                offset.magnitude,
                _obstacleLayer);
        }
        
        public RaycastHit[] GetObstaclesAlongPath(Vector3 start, Vector3 end, float playerRadius)
        {
            Vector3 offset = end - start;
            return Physics.SphereCastAll(start, playerRadius, offset.normalized, offset.magnitude, _obstacleLayer);
        }
    }
}