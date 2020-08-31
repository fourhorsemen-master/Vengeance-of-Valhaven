using UnityEngine;

public class DisengageObject : StaticAbilityObject
{
    public AudioSource disengageSound = null;

    public override float StickTime => disengageSound.clip.length;

    public static void Create(Transform casterTransform)
    {
        DisengageObject prefab = AbilityObjectPrefabLookup.Instance.DisengageObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
