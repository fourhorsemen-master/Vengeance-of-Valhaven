using UnityEngine;

public class LeechingStrikeObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hitAudioSource = null;

    [SerializeField]
    private Color slashColor = new Color();

    public override float StickTime => hitAudioSource.clip.length;

    public static LeechingStrikeObject Create(Vector3 position, Quaternion rotation)
    {
        LeechingStrikeObject prefab = AbilityObjectPrefabLookup.Instance.LeechingStrikeObjectPrefab;
        LeechingStrikeObject leechingStrikeObject = Instantiate(prefab, position, rotation);
        SlashObject.Create(position, rotation, leechingStrikeObject.slashColor);

        return leechingStrikeObject;
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }
}
