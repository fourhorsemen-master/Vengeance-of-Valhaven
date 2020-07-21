using System;
using UnityEngine;

public class NetThrowObject : ProjectileObject
{
    [SerializeField]
    private AudioSource landAudioSource = null;

    public static NetThrowObject Create(Actor caster, float throwVelocity, float throwAngle, float projectileTime)
    {        
        NetThrowObject prefab = AbilityObjectPrefabLookup.Instance.NetThrowObjectPrefab;
        NetThrowObject netThrowObject = Instantiate(prefab, caster.transform.position, caster.transform.rotation);

        netThrowObject.InitialiseProjectile(caster, null, throwVelocity * Mathf.Cos(throwAngle), throwVelocity * Mathf.Sin(throwAngle));
        netThrowObject.WaitAndAct(projectileTime, () =>
            {
                // netThrowObject.PlayLandingSound();
                netThrowObject.StopProjectile();
            }  
        );

        return netThrowObject;
    }

    // override method for OnTriggerEnter to handle collision properties and action on hitting the scene.

    private void PlayLandingSound()
    {
        landAudioSource.Play();
        this.WaitAndAct(landAudioSource.clip.length, () => Destroy(gameObject));
    }
}
