using UnityEngine;

public class ParryObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource initialSound = null;

    [SerializeField]
    private AudioSource parrySound = null;

    private float duration;

    public override float StickTime => Mathf.Max(initialSound.clip.length, duration + parrySound.clip.length);

    public static ParryObject Create(Transform transform, float duration, Subject onParry)
    {
        ParryObject parryObject = Instantiate(AbilityObjectPrefabLookup.Instance.ParryObjectPrefab, transform);
        parryObject.duration = duration;
        onParry.Subscribe(parryObject.parrySound.Play);

        return parryObject;
    }
}
