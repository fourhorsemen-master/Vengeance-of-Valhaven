using FMODUnity;
using UnityEngine;

public class CustomCamera : Singleton<CustomCamera>
{
    [Header("Camera settings")]
    [SerializeField]
    private StudioListener listener = null;

    [SerializeField, Range(0, 50)]
    private float height = 10;

    [SerializeField, Range(10, 80)]
    private float angle = 40;

    [SerializeField, Range(0f, 1f)]
    private float mouseFollowFactor = 0f;

    [SerializeField, Range(0f, 1f)]
    private float smoothFactor = 0.1f;

    [Header("Shake levels")]
    [SerializeField, Range(0, 50)]
    private float smallShakeStrength = 10;

    [SerializeField, Range(0, 1)]
    private float smallShakeDuration = 10;

    [SerializeField, Range(0, 50)]
    private float mediumShakeStrength = 10;

    [SerializeField, Range(0, 1)]
    private float mediumShakeDuration = 10;

    [SerializeField, Range(0, 50)]
    private float bigShakeStrength = 10;

    [SerializeField, Range(0, 1)]
    private float bigShakeDuration = 10;

    private GameObject target;

    private Vector3 desiredPosition = Vector3.zero;
    private Quaternion desiredRotation = Quaternion.identity;
    private Quaternion defaultRotation;

    private readonly CameraShakeManager shakeManager = new CameraShakeManager(4);

    private Pole orientation;

    private Transform transformOverride = null;
    private float? smoothFactorOverride = null;

    private float HorizontalMouseOffset => Input.mousePosition.x / Screen.width - 0.5f;
    private float VerticalMouseOffset => Input.mousePosition.y / Screen.height - 0.5f;

    protected override void Awake()
    {
        base.Awake();
        orientation = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.CameraOrientation;
    }

    private void Start()
    {
        target = ActorCache.Instance.Player.gameObject;

        FollowTarget(true);
        defaultRotation = Quaternion.Euler(new Vector3(angle, OrientationUtils.GetYRotation(orientation), 0));
        gameObject.transform.rotation = defaultRotation;

        listener.attenuationObject = target;
    }

    private void Update()
    {
        FollowTarget(false);
        shakeManager.ApplyShake(transform);
    }

    public void AddShake(ShakeIntensity intensity)
    {
        switch (intensity)
        {
            case ShakeIntensity.Low:
                shakeManager.AddCameraShake(smallShakeStrength, smallShakeDuration);
                break;
            case ShakeIntensity.Medium:
                shakeManager.AddCameraShake(mediumShakeStrength, mediumShakeDuration);
                break;
            case ShakeIntensity.High:
                shakeManager.AddCameraShake(bigShakeStrength, bigShakeDuration);
                break;
        }
    }

    public void OverrideDesiredTransform(Transform transformOverride, float? smoothFactorOverride = null)
    {
        this.transformOverride = transformOverride;
        this.smoothFactorOverride = smoothFactorOverride;
    }

    public void RemoveTransformOverride()
    {
        transformOverride = null;
        smoothFactorOverride = null;
    }

    private void FollowTarget(bool snap)
    {
        SetDesiredPosition();
        SetDesiredRotation();

        if (snap)
        {
            transform.position = desiredPosition;
            transform.rotation = desiredRotation;
            return;
        }

        float lerpAmount = 1 - Mathf.Exp(- Time.deltaTime / (smoothFactorOverride ?? smoothFactor));
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpAmount);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, lerpAmount);
    }

    private void SetDesiredPosition()
    {
        if (transformOverride)
        {
            desiredPosition = transformOverride.position;
            return;
        }

        float distanceFromFloorIntersect = height / (Mathf.Tan(Mathf.Deg2Rad * angle));
        Vector3 targetPosition = target.transform.position;

        float xOffset = 0;
        float zOffset = 0;
        float mouseXOffset = 0;
        float mouseZOffset = 0;

        switch (orientation)
        {
            case Pole.North:
                xOffset = 0;
                zOffset = -distanceFromFloorIntersect;
                mouseXOffset = HorizontalMouseOffset;
                mouseZOffset = VerticalMouseOffset;
                break;
            case Pole.East:
                xOffset = -distanceFromFloorIntersect;
                zOffset = 0;
                mouseXOffset = VerticalMouseOffset;
                mouseZOffset = -HorizontalMouseOffset;
                break;
            case Pole.South:
                xOffset = 0;
                zOffset = distanceFromFloorIntersect;
                mouseXOffset = -HorizontalMouseOffset;
                mouseZOffset = -VerticalMouseOffset;
                break;
            case Pole.West:
                xOffset = distanceFromFloorIntersect;
                zOffset = 0;
                mouseXOffset = -VerticalMouseOffset;
                mouseZOffset = HorizontalMouseOffset;
                break;
        }

        desiredPosition.Set(
            targetPosition.x + xOffset + (mouseXOffset * mouseFollowFactor),
            targetPosition.y + height,
            targetPosition.z +zOffset + (mouseZOffset * mouseFollowFactor)
        );
    }

    private void SetDesiredRotation()
    {
        if (transformOverride)
        {
            desiredRotation = transformOverride.rotation;
            return;
        }

        desiredRotation = defaultRotation;
    }
}
