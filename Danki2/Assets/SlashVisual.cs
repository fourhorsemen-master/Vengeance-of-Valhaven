using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVisual : MonoBehaviour
{

    [SerializeField]
    private float rotSpeed = 0f;

    [SerializeField]
    private float fadeSpeed = 0f;

    private float duration = 1f;

    private MeshRenderer meshRenderer;
    private Color desiredColor;

    private bool hasStartedCorrectly = false;

    private void Start()
    {
        desiredColor = new Color(1f, 1f, 1f, 0f);
        DoVisual();
    }

    public void DoVisual()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("Color", desiredColor);
        meshRenderer.enabled = true;
        hasStartedCorrectly = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStartedCorrectly)
        {
            if (duration < 0f)
            {
                Destroy(gameObject);
            }
            else
            {
                UpdateVisual();
            }
        }
        else
        {
            Debug.Log("Visual object attempting to update but has not started correctly. Check scene for visual objects, and remove them.");
        }
        
    }

    private void UpdateVisual()
    {
        desiredColor.a = Mathf.Lerp(0f, 1f, duration);
        meshRenderer.sharedMaterial.SetColor("_UnlitColor", desiredColor);
        transform.Rotate(0f, -rotSpeed * Time.deltaTime, 0f);
        duration -= fadeSpeed * Time.deltaTime;
    }
}
