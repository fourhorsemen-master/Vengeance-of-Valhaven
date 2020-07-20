using System;
using UnityEngine;

public class NetThrowObject : ProjectileObject
{
    [SerializeField]
    private AudioSource hitAudioSource = null;

    public static NetThrowObject Create(Quaternion rotation, float velocity, float throwAngle, float projectileTime)
    {
        NetThrowObject prefab = AbilityObjectPrefabLookup.Instance.NetThrowObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }

    public void PlayHitSound()
    {
        hitAudioSource.Play();
    }
}
