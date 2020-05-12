using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashVisual : MonoBehaviour
{
    [SerializeField, Range(-100f, 100f), Tooltip("How fast the effect will be spinning at the start. In degrees per second.")]
    private float initialRotationSpeed = 0f;
    [SerializeField, Range(-100f, 100f), Tooltip("How fast the effect will be spinning at he end. In degrees per second.")]
    private float endRotationSpeed = 0f;
    [SerializeField, Range(0f, 10f), Tooltip("How quickly the effect changes from start speed to end speed. In seconds.")]
    private float rotationTime = 0f;
    private float currentRotationLerpValue = 0f;

    [SerializeField, Range(0f, 10f), Tooltip("How large the effect will be at the start.")]
    private float initialScale = 0f;
    [SerializeField, Range(0f, 10f), Tooltip("How large the effect will be at the end.")]
    private float endScale = 0f;
    [SerializeField, Range(0f, 10f), Tooltip("How quickly the effect changes from start speed to end speed. In seconds.")]
    private float growTime = 0f;
    private float currentGrowLerpValue = 0f;

    [SerializeField, Range(0f, 10f), Tooltip("How quickly the effect will fade from fullly opaque to fully transparent. In seconds")]
    private float fadeTime = 0f;
    [SerializeField, Range(0f, 10f), Tooltip("How long before the fade beins. In seconds")]
    private float fadeDelay = 0f;
    private float currentFadeDelayTimer = 0f;
    private float currentFadeLerpValue = 0f;

    private Material material;
    private Color desiredColour = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponentInChildren<MeshRenderer>().material;

        //Invert these values, so they resemble seconds; as described in the tooltips.
        rotationTime = 1 / rotationTime;
        growTime = 1 / growTime;
        fadeTime = 1 / fadeTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * Mathf.Lerp(initialScale, endScale, currentGrowLerpValue);
        currentGrowLerpValue += growTime * Time.deltaTime;

        transform.Rotate(Vector3.up, Mathf.Lerp(initialRotationSpeed, endRotationSpeed, currentRotationLerpValue));
        currentRotationLerpValue += rotationTime * Time.deltaTime;

        material.SetColor("_UnlitColor", desiredColour);
        desiredColour = Color.Lerp(Color.white, Color.clear, currentFadeLerpValue);

        if (currentFadeDelayTimer > fadeDelay)
        {
            currentFadeLerpValue += fadeTime * Time.deltaTime;
        }
        currentFadeDelayTimer += Time.deltaTime;
    }
}
