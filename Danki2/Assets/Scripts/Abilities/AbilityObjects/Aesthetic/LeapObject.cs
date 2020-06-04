using UnityEngine;

public class LeapObject : StaticAbilityObject
{
    public AudioSource leapSound = null;

    public override float StickTime => leapSound.clip.length;

    public static void Create(Transform casterTransform)
    {
        LeapObject prefab = AbilityObjectPrefabLookup.Instance.LeapObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
