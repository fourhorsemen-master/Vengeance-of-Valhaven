using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class CustomCamera : Singleton<CustomCamera>
{
    // Camera settings
    [SerializeField]
    private GameObject target = null;

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

    // Shake levels
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

    private Vector3 desiredPosition;

    private CameraShakeManager shakeManager = new CameraShakeManager(4);

    private Pole orientation;

    private static readonly Dictionary<Pole, float> orientationToYRotation = new Dictionary<Pole, float>
    {
        [Pole.North] = 0,
        [Pole.East] = 90,
        [Pole.South] = 180,
        [Pole.West] = 270
    };

    protected override void Awake()
    {
        base.Awake();
        orientation = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.CameraOrientation;
    }

    private void Start()
    {
        FollowTarget(true);
        gameObject.transform.eulerAngles = new Vector3(angle, orientationToYRotation[orientation], 0);

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

    private void FollowTarget(bool snap)
    {
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
                mouseXOffset = Input.mousePosition.x / Screen.width - 0.5f;
                mouseZOffset = Input.mousePosition.y / Screen.height - 0.5f;
                break;
            case Pole.East:
                xOffset = -distanceFromFloorIntersect;
                zOffset = 0;
                mouseXOffset = Input.mousePosition.y / Screen.height - 0.5f;
                mouseZOffset = -Input.mousePosition.x / Screen.width - 0.5f;
                break;
            case Pole.South:
                xOffset = 0;
                zOffset = distanceFromFloorIntersect;
                mouseXOffset = -Input.mousePosition.x / Screen.width - 0.5f;
                mouseZOffset = -Input.mousePosition.y / Screen.height - 0.5f;
                break;
            case Pole.West:
                xOffset = distanceFromFloorIntersect;
                zOffset = 0;
                mouseXOffset = -Input.mousePosition.y / Screen.height - 0.5f;
                mouseZOffset = Input.mousePosition.x / Screen.width - 0.5f;
                break;
        }

        desiredPosition.Set(
            targetPosition.x + xOffset + (mouseXOffset * mouseFollowFactor),
            targetPosition.y + height,
            targetPosition.z +zOffset + (mouseZOffset * mouseFollowFactor)
        );

        if (snap)
        {
            transform.position = desiredPosition;
            return;
        }

        float lerpAmount = 1 - Mathf.Exp(- Time.deltaTime / smoothFactor);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpAmount);
    }
}
