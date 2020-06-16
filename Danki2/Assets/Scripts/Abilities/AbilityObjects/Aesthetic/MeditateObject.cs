using UnityEngine;

public class MeditateObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource meditateSound = null;

    [SerializeField, Range(0, 2)]
    private float heartbeatInterval = 1f;

    [SerializeField, Range(0, 1)]
    private float initialVolume = 0f;

    [SerializeField, Range(0, 1)]
    private float finalVolume = 1f;

    private float maxDuration;

    private Repeater repeater;

    private void Start()
    {
        meditateSound.volume = initialVolume;
        repeater = new Repeater(heartbeatInterval, PlayHeartbeat);
    }

    private void Update()
    {
        // Linear function from initialVolume to finalVolume.
        meditateSound.volume += (Time.deltaTime / maxDuration) * (finalVolume - initialVolume);

        repeater.Update();
    }

    public static MeditateObject Create(Transform casterTransform, float maxDuration)
    {
        MeditateObject prefab = AbilityObjectPrefabLookup.Instance.MeditateObjectPrefab;
        MeditateObject meditateObject = Instantiate(prefab, casterTransform);
        meditateObject.maxDuration = maxDuration;

        return meditateObject;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void PlayHeartbeat()
    {
        meditateSound.Play();
        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
