using UnityEngine;

public class SmashObject : StaticAbilityObject
{
    public AudioSource smashSound = null;

    public override float StickTime => smashSound.clip.length;

    public static void Create(Vector3 position)
    {
        SmashObject prefab = AbilityObjectPrefabLookup.Instance.SmashObjectPrefab;
        Instantiate(prefab, position, Quaternion.identity);
    }
}