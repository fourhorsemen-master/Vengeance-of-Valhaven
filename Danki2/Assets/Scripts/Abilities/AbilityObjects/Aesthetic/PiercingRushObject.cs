using UnityEngine;

public class PiercingRushObject : StaticAbilityObject
{
    public AudioSource piercingRushSound = null;

    public override float StickTime => piercingRushSound.clip.length;

    public static void Create(Transform casterTransform)
    {
        PiercingRushObject prefab = AbilityObjectPrefabLookup.Instance.PiercingRushObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
