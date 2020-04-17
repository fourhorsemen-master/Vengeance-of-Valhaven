using UnityEngine;

public class SlashObject : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0f;

    [SerializeField]
    private float duration = 0f;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    [SerializeField]
    private AudioSource thudAudioSource = null;

    private float remainingDuration;

    private Color desiredColor = new Color(1f, 1f, 1f, 0f);

    private void Start()
    {
        remainingDuration = duration;
        meshRenderer.material.SetColor("Color", desiredColor);
        meshRenderer.enabled = true;

        transform.LookAt(MouseGamePositionFinder.Instance.GetFlooredMouseGamePosition());
    }

    private void Update()
    {
        if (remainingDuration < 0f)
        {
            Destroy(gameObject);
        }

        UpdateVisual();
    }

    public void PlayThudSound()
    {
        thudAudioSource.Play();
    }

    private void UpdateVisual()
    {
        desiredColor.a = Mathf.Lerp(0f, 1f, remainingDuration / duration);
        meshRenderer.sharedMaterial.SetColor("_UnlitColor", desiredColor);
        transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        remainingDuration -= Time.deltaTime;
    }
}
