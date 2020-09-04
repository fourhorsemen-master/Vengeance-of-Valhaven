using UnityEngine;

public class HamstringObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hamstringSound = null;

    public override float StickTime => hamstringSound.clip.length;

    public static void Create(Transform transform)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.HamstringObjectPrefab, transform);
    }
}
