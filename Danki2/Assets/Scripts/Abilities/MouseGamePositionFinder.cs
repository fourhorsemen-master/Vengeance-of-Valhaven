using UnityEngine;
using UnityEngine.AI;

public class MouseGamePositionFinder : Singleton<MouseGamePositionFinder>
{
    [SerializeField]
    private float heightOffset = 0;

    /// <summary>
    /// Returns the position that a ray from the camera to the mouse collides with a collider, factoring in an offset height.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMouseGamePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (NavMesh.SamplePosition(raycastHit.point, out NavMeshHit navMeshHit, heightOffset, NavMesh.AllAreas))
            {
                Vector3 position = navMeshHit.position;
                position.y += heightOffset;
                return position;
            }

            return raycastHit.point;
        }
        else
        {
            Debug.LogError("Unable to find mouse position in world");
            return default;
        }
    }
}
