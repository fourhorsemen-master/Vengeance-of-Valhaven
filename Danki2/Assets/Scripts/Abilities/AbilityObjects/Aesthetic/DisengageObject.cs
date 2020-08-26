using UnityEngine;

public class DisengageObject : StaticAbilityObject
{
    public AudioSource dashSound = null;

    public override float StickTime => dashSound.clip.length;

    public static void Create(Transform casterTransform)
    {
        DashObject prefab = AbilityObjectPrefabLookup.Instance.DashObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
