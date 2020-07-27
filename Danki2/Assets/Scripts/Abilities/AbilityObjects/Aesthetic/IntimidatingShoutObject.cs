using UnityEngine;

public class IntimidatingShoutObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource audioSource = null;

    public override float StickTime => audioSource.clip.length;

    public static void Create(Transform transform)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.IntimidatingShoutObject, transform);
    }
}
