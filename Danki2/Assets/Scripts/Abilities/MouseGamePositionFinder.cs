using UnityEngine;
using UnityEngine.AI;

public class MouseGamePositionFinder : Singleton<MouseGamePositionFinder>
{
    [SerializeField]
    private float heightOffset = 0;

    /// <summary>
    /// Casts a ray from the camera through the mouse to get the position where it collides with a collider.
    /// If this point is close to the navMesh we take the closest navMesh position and add a y-offset.
    /// </summary>
    /// <returns>True if the mouse is within the scene.</returns>
    public bool TryGetMouseGamePosition(out Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            position = default;
            return false;
        }

        if (NavMesh.SamplePosition(raycastHit.point, out _, 0.65f, NavMesh.AllAreas))
        {
            position = raycastHit.point;
            Vector3 positionToCamera = Camera.main.transform.position - position;
            Vector3 offset = positionToCamera * heightOffset / positionToCamera.y;

            position += offset;
        }
        else
        {
            position = raycastHit.point;
        }

        return true;
    }

    /// <summary>
    /// Get the mouse position on a horizontal plane at a given height.
    /// </summary>
    /// <param name="planeHeight"></param>
    /// <param name="includeOffset">Include the global height offset in addition to the planeHeight. Default false.</param>
    /// <returns></returns>
    public Vector3 GetMousePlanePosition(float planeHeight, bool includeOffset = false)
    {
        if (includeOffset) planeHeight += heightOffset;

        Plane plane = new Plane(Vector3.down, planeHeight);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out float distanceAlongRay);

        return ray.GetPoint(distanceAlongRay);
    }
}
