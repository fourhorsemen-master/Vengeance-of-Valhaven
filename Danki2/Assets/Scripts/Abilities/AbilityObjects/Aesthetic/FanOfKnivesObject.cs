using System;
using UnityEngine;

public class FanOfKnivesObject : ProjectileObject
{
    [SerializeField]
    private AudioSource collisionSound = null;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.FanOfKnivesObjectPrefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed)
            .SetSticky();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != caster.gameObject)
        {
            collisionSound.Play();
            trailRenderer.emitting = false;
        }

        base.OnTriggerEnter(other);
    }
}
