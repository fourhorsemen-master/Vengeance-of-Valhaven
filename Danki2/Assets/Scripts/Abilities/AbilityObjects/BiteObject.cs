using UnityEngine;

public class BiteObject : StaticAbilityObject
{
    public AudioSource biteSound = null;

    public override float StickTime => biteSound.clip.length;

    public void Awake()
    {
        biteSound.time = 0.5f;
        biteSound.Play();
    }

    public static void Create(Transform abilitySource)
    {
        BiteObject prefab = AbilityObjectPrefabLookup.Instance.BiteObjectPrefab;
        Instantiate(prefab, abilitySource);
    }
}
