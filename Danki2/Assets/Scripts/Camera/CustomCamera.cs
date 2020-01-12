using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject target = null;

    [SerializeField, Range(0, 50)]
    private float height = 10;

    [SerializeField, Range(10, 80)]
    private float angle = 40;

    [SerializeField, Range(0f, 1f)]
    private float mouseFollowFactor = 0f;

    [SerializeField, Range(0f, 1f)]
    private float smoothFactor = 0f;

    private Vector3 desiredPosition;

    private CameraShakeManager shakeManager = new CameraShakeManager(4);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            shakeManager.AddCameraShake(15f, 0.3f);
        }

        UpdateRotation();
        FollowTarget();
        transform.Translate(shakeManager.GetShakeVector(), Space.World);
    }

    private void UpdateRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(angle, 0, 0);
    }

    private void FollowTarget()
    {
        float zDistanceFromFloorIntersect = height / (Mathf.Tan(Mathf.Deg2Rad * angle));
        Vector3 mousepoint = MouseGamePositionFinder.Instance.GetMouseGamePosition();
        desiredPosition.x = Mathf.Lerp(target.transform.position.x, mousepoint.x, mouseFollowFactor);
        desiredPosition.y = target.transform.position.y + height;
        desiredPosition.z = Mathf.Lerp(target.transform.position.z - zDistanceFromFloorIntersect, mousepoint.z, mouseFollowFactor);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f - smoothFactor);
    }

    public void AddShake(float strength, float duration, float interval = CameraShake.DefaultInterval)
    {
        shakeManager.AddCameraShake(strength, duration, interval);
    }
}
