using System;
using UnityEngine;

public class HookObject : ProjectileObject
{
    [SerializeField]
    private AudioSource hookHitAudio = null;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    public static HookObject Fire(Actor caster, Action<GameObject> collisionCallback, Action missCallback, float speed, Vector3 position, Quaternion rotation, float maxRange)
    {
        float stickTime = maxRange / speed;

        HookObject hookObject = Instantiate(AbilityObjectPrefabLookup.Instance.HookObjectPrefab, position, rotation);
        hookObject.InitialiseProjectile(caster, collisionCallback, speed)
            .DestroyAfterTime(stickTime, missCallback);

        hookObject.SetSticky();

        return hookObject;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if (!ReferenceEquals(this.caster.gameObject, collider.gameObject))
        {
            trailRenderer.enabled = false;
        }
    }

    public void PlayHitAudio()
    {
        hookHitAudio.Play();
    }
}
