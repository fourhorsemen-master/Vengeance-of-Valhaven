using UnityEngine;

public class MouseGamePositionFinder : Singleton<MouseGamePositionFinder>
{
    [SerializeField]
    private float _planeHeight = 0;

    private Plane _plane;

    protected override void Awake()
    {
        base.Awake();

        _plane = new Plane(Vector3.up, _planeHeight * Vector3.up);
    }

    /// <summary>
    /// Returns position of mouse when projected to a horizontal plane at the height set on
    /// this component.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMouseGamePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }
        else
        {
            Debug.LogError("Unable to find mouse position in world");
            return default;
        }
    }

    /// <summary>
    /// Returns the position of the mouse when projected onto the horizontal plane, of the
    /// height set on this component, and then casts down to the floor.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetFlooredMouseGamePosition()
    {
        Vector3 mouseGamePosition = GetMouseGamePosition();
        mouseGamePosition.y = 0;
        return mouseGamePosition;
    }
}
