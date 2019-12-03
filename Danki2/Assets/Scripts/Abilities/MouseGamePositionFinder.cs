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
}
