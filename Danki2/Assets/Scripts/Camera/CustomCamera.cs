using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject target = null;

    [SerializeField, Range(0, 50)]
    private float height = 10;

    [SerializeField, Range(10, 80)]
    private float angle = 40;

    private void Update()
    {
        UpdateRotation();
        FollowTarget();
    }

    private void UpdateRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(angle, 0, 0);
    }

    private void FollowTarget()
    {
        float zDistanceFromFloorIntersect = height / (Mathf.Tan(Mathf.Deg2Rad * angle));
        float newX = target.transform.position.x;
        float newY = target.transform.position.y + height;
        float newZ = target.transform.position.z - zDistanceFromFloorIntersect;

        transform.position = new Vector3(newX, newY, newZ);
    }
}
