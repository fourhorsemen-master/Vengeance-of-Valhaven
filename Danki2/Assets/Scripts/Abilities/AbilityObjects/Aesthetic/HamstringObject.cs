using UnityEngine;

public class HamstringObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource hamstringSound = null;

    public override float StickTime => hamstringSound.clip.length;

    public static void Create(Transform transform, Vector3 target)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.HamstringObjectPrefab, transform)
            .gameObject.transform.LookAt(target, Vector3.up);
    }
}
