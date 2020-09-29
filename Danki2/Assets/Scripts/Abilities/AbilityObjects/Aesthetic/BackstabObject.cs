using UnityEngine;

public class BackstabObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hitAudioSource = null;

    public override float StickTime => hitAudioSource.clip.length;

    public static BackstabObject Create(Vector3 position, Quaternion rotation)
    {
        BackstabObject prefab = AbilityObjectPrefabLookup.Instance.BackstabObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }
}
