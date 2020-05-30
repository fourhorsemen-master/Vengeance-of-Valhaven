﻿using UnityEngine;

public class LeapObject : StaticAbilityObject
{
    public AudioSource leapSound = null;

    public override float StickTime { get; set; }

    public void Awake()
    {
        StickTime = leapSound.clip.length;
    }

    public static void Create(Transform casterTransform)
    {
        LeapObject prefab = AbilityObjectPrefabLookup.Instance.LeapObjectPrefab;
        Instantiate(prefab, casterTransform);
    }
}
