using UnityEngine;

public class LungeObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hitAudioSource = null;

    public override float StickTime => 2f;

    public static LungeObject Create(Vector3 position, Quaternion rotation)
    {
        LungeObject prefab = AbilityObjectPrefabLookup.Instance.LungeObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }
}

