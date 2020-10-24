using UnityEngine;

public class HamstringObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hamstringSound = null;

    public override float StickTime => hamstringSound.clip.length;

    public static HamstringObject Create(Vector3 position, Quaternion rotation)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.HamstringObjectPrefab, position, rotation);
    }

    public void PlayHitSound()
    {
        hamstringSound.Play();
    }
}
