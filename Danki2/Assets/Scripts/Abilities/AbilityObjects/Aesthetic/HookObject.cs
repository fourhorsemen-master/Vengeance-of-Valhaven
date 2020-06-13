using UnityEngine;

public class HookObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hookSound = null;

    public override float StickTime => hookSound.clip.length;

    public static void Create(Transform casterTransform)
    {
        HookObject prefab = AbilityObjectPrefabLookup.Instance.HookObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
