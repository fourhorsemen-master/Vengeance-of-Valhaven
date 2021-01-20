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

    public static void Create(Vector3 position, Quaternion rotation)
    {
        BiteObject prefab = AbilityObjectPrefabLookup.Instance.BiteObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}
