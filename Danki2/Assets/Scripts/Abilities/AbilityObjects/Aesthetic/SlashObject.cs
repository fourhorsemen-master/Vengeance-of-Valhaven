using Assets.Scripts.UnityHelpers.Extensions;
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
    private AudioSource hitAudioSource = null;

    private float remainingDuration;

    private Color desiredColor = new Color(1f, 1f, 1f, 0f);

    internal static SlashObject Create(Vector3 position, Quaternion rotation)
    {
        SlashObject prefab = AbilityObjectPrefabLookup.Instance.SlashObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }

    private void Start()
    {
        remainingDuration = duration;
        meshRenderer.material.SetColour(desiredColor);
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
        meshRenderer.sharedMaterial.SetUnlitColour(desiredColor);
        transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        remainingDuration -= Time.deltaTime;
    }
}
