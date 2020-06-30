using UnityEngine;

public class BackstabObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hitAudioSource = null;

    [SerializeField]
    private Color slashColor = new Color();

    public override float StickTime => hitAudioSource.clip.length;

    public static BackstabObject Create(Vector3 position, Quaternion rotation)
    {
        BackstabObject prefab = AbilityObjectPrefabLookup.Instance.BackstabObjectPrefab;
        BackstabObject backstabObject = Instantiate(prefab, position, rotation);
        SlashObject.Create(position, rotation, backstabObject.slashColor);

        return backstabObject;
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }
}
