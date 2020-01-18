﻿using UnityEngine;

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
        // TODO: Remove the following test key.
        if (Input.GetKeyDown(KeyCode.O))
        {
            shakeManager.AddCameraShake(15f, 0.3f);
        }

        UpdateRotation();
        FollowTarget();
        shakeManager.ApplyShake(transform);
    }

    private void UpdateRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(angle, 0, 0);
    }

    private void FollowTarget()
    {
        float zDistanceFromFloorIntersect = height / (Mathf.Tan(Mathf.Deg2Rad * angle));
        var targetPosition = target.transform.position;

        var mouseOffsetH = Input.mousePosition.x / Screen.width - 0.5f;
        var mouseOffsetV = Input.mousePosition.y / Screen.height - 0.5f;

        desiredPosition.Set(
            targetPosition.x + (mouseOffsetH * mouseFollowFactor),
            targetPosition.y + height,
            targetPosition.z - zDistanceFromFloorIntersect + (mouseOffsetV * mouseFollowFactor)
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f - smoothFactor);
    }

    public void AddShake(float strength, float duration, float interval = CameraShake.DefaultInterval)
    {
        shakeManager.AddCameraShake(strength, duration, interval);
    }
}
