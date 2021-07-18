using UnityEngine;
using UnityEngine.AI;

public class MouseGamePositionFinder : Singleton<MouseGamePositionFinder>
{
    [SerializeField]
    private float navmeshClearance = 0;

    private float heightOffset;

    private Plane plane = new Plane(Vector3.down, 0f);

    private void Start()
    {
        heightOffset = ActorCache.Instance.Player.Height;
    }

    /// <summary>
    /// Casts a ray from the camera through the mouse to see if it hits a collider.
    /// </summary>
    /// <param name="collider"> The collider that the mouse point hit </param>
    /// <param name="position"> Point of the collision </param>
    /// <param name="layerMask">
    ///     An optional layer mask used to selectively ignore colliders when casting, depending on their layers.
    ///     See here:
    ///     https://docs.unity3d.com/ScriptReference/Physics.Raycast.html,
    ///     and here:
    ///     https://docs.unity3d.com/Manual/Layers.html
    ///     for more information.
    ///     This will default to include all colliders.
    /// </param>
    /// <returns>True if the mouse did hit a collider.</returns>
    public bool TryGetCollider(out Collider collider, out Vector3 position, int layerMask = int.MaxValue)
    {
        if (Physics.Raycast(GetCameraToMouseRay(), out RaycastHit raycastHit, Mathf.Infinity, layerMask))
        {
            collider = raycastHit.collider;
            position = raycastHit.point;
            return true;
        }

        collider = null;
        position = default;
        return false;
    }

    /// <summary>
    /// Gets the <paramref name="floorPosition"/> and the <paramref name="offsetPosition"/> (the position with the
    /// global height offset) of the mouse on the navmesh, if the mouse is sufficiently close to the navmesh.
    /// </summary>
    public bool TryGetNavMeshPositions(out Vector3 floorPosition, out Vector3 offsetPosition)
    {
        floorPosition = default;
        offsetPosition = default;

        int layerMask = LayerUtils.GetInvertedLayerMask(LayerUtils.GetLayerMask(Layer.Actors));
        if (!Physics.Raycast(GetCameraToMouseRay(), out RaycastHit raycastHit, Mathf.Infinity, layerMask)) return false;

        if (!NavMesh.SamplePosition(raycastHit.point, out _, navmeshClearance, NavMesh.AllAreas)) return false;

        floorPosition = raycastHit.point;
        Vector3 positionToCamera = Camera.main.transform.position - floorPosition;
        Vector3 offset = positionToCamera * heightOffset / positionToCamera.y;
        offsetPosition = floorPosition + offset;

        return true;
    }

    /// <summary>
    /// Gets the <paramref name="floorPosition"/> and the <paramref name="offsetPosition"/> (the position with the
    /// global height offset) of the mouse at the given <paramref name="planeHeight"/>.
    /// </summary>
    public void GetPlanePositions(float planeHeight, out Vector3 floorPosition, out Vector3 offsetPosition)
    {
        Ray ray = GetCameraToMouseRay();

        plane.distance = planeHeight;
        plane.Raycast(ray, out float distanceAlongRay);
        floorPosition = ray.GetPoint(distanceAlongRay);

        plane.distance += heightOffset;
        plane.Raycast(ray, out float offsetDistanceAlongRay);
        offsetPosition = ray.GetPoint(offsetDistanceAlongRay);
    }

    private Ray GetCameraToMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
