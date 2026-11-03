using UnityEngine;

namespace Controllers
{
    public class PathChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private int _raycastCount = 10;
        
        public bool IsPathClear(Vector3 start, Vector3 end, float playerRadius)
        {
            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);
            
            for (int i = 0; i < _raycastCount; i++)
            {
                float t = (float)i / (_raycastCount - 1);
                Vector3 point = Vector3.Lerp(start, end, t);
                
                if (Physics.CheckSphere(point, playerRadius, _obstacleLayer))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
