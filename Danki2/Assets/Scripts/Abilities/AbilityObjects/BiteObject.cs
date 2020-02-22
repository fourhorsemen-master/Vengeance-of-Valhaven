using System;
using UnityEngine;

public class BiteObject : StaticObject
{
    public AudioSource biteSound = null;

    public void Start()
    {
        float stickTime = biteSound.clip.length;
        InitialiseAbility(stickTime);

        biteSound.Play();
    }

    public static void Create(Vector3 position, Quaternion rotation)
    {
        BiteObject prefab = AbilityObjectPrefabLookup.Instance.BiteObjectPrefab;
        Instantiate(prefab, position, rotation);
    }
}
