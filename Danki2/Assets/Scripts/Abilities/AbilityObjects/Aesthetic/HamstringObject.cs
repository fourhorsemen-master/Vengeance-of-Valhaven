using UnityEngine;

public class HamstringObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hamstringSound = null;

    public override float StickTime => hamstringSound.clip.length;

    public static void Create(Vector3 position, Quaternion rotation)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.HamstringObjectPrefab, position, rotation);
    }
}
