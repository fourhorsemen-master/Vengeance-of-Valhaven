using System;
using UnityEngine;

public class GrapplingHookObject : ProjectileObject
{
    [SerializeField]
    private TrailRenderer trailRenderer = null;

    public static GrapplingHookObject Fire(Actor caster, Action<GameObject> collisionCallback, Action missCallback, float speed, Vector3 position, Quaternion rotation, float maxRange)
    {
        float stickTime = maxRange / speed;

        GrapplingHookObject grapplingHookObject = Instantiate(AbilityObjectPrefabLookup.Instance.GrapplingHookObjectPrefab, position, rotation);
        grapplingHookObject.InitialiseProjectile(caster, collisionCallback, speed)
            .DestroyAfterTime(stickTime, missCallback);

        grapplingHookObject.SetSticky(1f);

        return grapplingHookObject;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if (!ReferenceEquals(this.caster.gameObject, collider.gameObject))
        {
            trailRenderer.enabled = false;
        }
    }
}
