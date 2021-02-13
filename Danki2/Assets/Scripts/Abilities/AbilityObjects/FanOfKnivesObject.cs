using System;
using UnityEngine;

public class FanOfKnivesObject : ProjectileObject
{
    [SerializeField]
    private TrailRenderer trailRenderer = null;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        Instantiate(AbilityObjectPrefabLookup.Instance.FanOfKnivesObjectPrefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed)
            .SetSticky(5f);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != caster.gameObject)
        {
            trailRenderer.emitting = false;
        }

        base.OnTriggerEnter(other);
    }
}
