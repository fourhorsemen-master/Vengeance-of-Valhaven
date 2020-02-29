using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVisual : MonoBehaviour
{

    [SerializeField]
    private float _rotationSpeed = 0f;

    [SerializeField]
    private float _duration = 0f;

    private float _remainingDuration;

    private MeshRenderer _meshRenderer;
    private Color _desiredColor = new Color(1f, 1f, 1f, 0f);

    private void Start()
    {
        _remainingDuration = _duration;
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _meshRenderer.material.SetColor("Color", _desiredColor);
        _meshRenderer.enabled = true;

        transform.LookAt(MouseGamePositionFinder.Instance.GetFlooredMouseGamePosition());
    }

    void Update()
    {
        //We are a child object, so destroy parent.
        if (_remainingDuration < 0f)
        {
            Destroy(transform.parent.gameObject);
        }

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _desiredColor.a = Mathf.Lerp(0f, 1f, _remainingDuration / _duration);
        _meshRenderer.sharedMaterial.SetColor("_UnlitColor", _desiredColor);
        transform.Rotate(0f, -_rotationSpeed * Time.deltaTime, 0f);
        _remainingDuration -= Time.deltaTime;
    }
}
