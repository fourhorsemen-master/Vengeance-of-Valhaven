using UnityEngine;

public class LeechingStrikeObject : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0f;

    [SerializeField]
    private float duration = 0f;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    [SerializeField]
    private AudioSource hitAudioSource = null;

    private float remainingDuration;

    private Color desiredColor = new Color(1f, 1f, 1f, 0f);

    internal static LeechingStrikeObject Create(Vector3 position, Quaternion rotation)
    {
        LeechingStrikeObject prefab = AbilityObjectPrefabLookup.Instance.LeechingStrikeObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }

    private void Start()
    {
        remainingDuration = duration;
        meshRenderer.material.SetColor("Color", desiredColor);
        meshRenderer.enabled = true;
    }

    private void Update()
    {
        if (remainingDuration < 0f)
        {
            Destroy(gameObject);
        }

        UpdateVisual();
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }

    private void UpdateVisual()
    {
        desiredColor.a = Mathf.Lerp(0f, 1f, remainingDuration / duration);
        meshRenderer.sharedMaterial.SetColor("_UnlitColor", desiredColor);
        transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        remainingDuration -= Time.deltaTime;
    }
}
