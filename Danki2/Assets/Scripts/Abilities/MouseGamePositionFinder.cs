using UnityEngine;
using UnityEngine.AI;

public class MouseGamePositionFinder : Singleton<MouseGamePositionFinder>
{
    [SerializeField]
    private float heightOffset = 0;

    [SerializeField]
    private float navmeshClearance = 0;

    private Plane plane = new Plane(Vector3.down, 0f);

    public float HeightOffset => heightOffset;

    /// <summary>
    /// Casts a ray from the camera through the mouse to get the position where it collides with a collider.
    /// If this point is close to the navMesh, we add a vector that moves the point towards the camera such that y-value is increased by heightOffset.
    /// This is so that if you click on the 'floor', you'll fire horizontally.
    /// </summary>
    /// <param name="position"> Point of the collision </param>
    /// <param name="collider"> The collider that the mouse point hit </param>
    /// <param name="layerMask">
    ///     An optional layer mask used to selectively ignore colliders when casting, depending on their layers, see here:
    ///     https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
    ///     and here:
    ///     https://docs.unity3d.com/Manual/Layers.html
    ///     for more information.
    ///     This will default to include all colliders.
    /// </param>
    /// <returns>True if the mouse did hit a collider.</returns>
    public bool TryGetMouseGamePosition(out Vector3 position, out Collider collider, int layerMask = int.MaxValue)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
        {
            position = default;
            collider = null;
            return false;
        }

        if (NavMesh.SamplePosition(raycastHit.point, out _, navmeshClearance, NavMesh.AllAreas))
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

        collider = raycastHit.collider;
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
        plane.distance = planeHeight;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out float distanceAlongRay);

        return ray.GetPoint(distanceAlongRay);
    }
}
