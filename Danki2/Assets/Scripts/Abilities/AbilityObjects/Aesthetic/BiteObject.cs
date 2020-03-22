using System.Collections;
using UnityEngine;

public class BiteObject : StaticAbilityObject
{
    public AudioSource biteSound = null;
    public override float StickTime { get; set; }

    public void Awake()
    {
        StickTime = biteSound.clip.length;
    }

    public static void Create(Transform casterTransform)
    {
        BiteObject prefab = AbilityObjectPrefabLookup.Instance.BiteObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
