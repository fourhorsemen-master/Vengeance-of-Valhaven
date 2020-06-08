using UnityEngine;

public class BashObject : StaticAbilityObject
{
    public AudioSource bashSound = null;

    public override float StickTime => bashSound.clip.length;

    public static void Create(Vector3 position, Quaternion rotation)
    {
        BashObject prefab = AbilityObjectPrefabLookup.Instance.BashObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}
