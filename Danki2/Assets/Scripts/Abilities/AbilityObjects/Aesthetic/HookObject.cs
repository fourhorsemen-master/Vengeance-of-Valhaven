using System;
using UnityEngine;

public class HookObject : ProjectileObject
{
    [SerializeField]
    private AudioSource hookHitAudio = null;

    public static HookObject Fire(Actor caster, Action<GameObject> collisionCallback, Action missCallback, float speed, Vector3 position, Quaternion rotation, float maxRange)
    {
        float stickTime = maxRange / speed;

        HookObject hookObject = Instantiate(AbilityObjectPrefabLookup.Instance.HookObjectPrefab, position, rotation);
        hookObject.InitialiseProjectile(caster, collisionCallback, speed)
            .DestroyAfterTime(stickTime, missCallback);

        hookObject.SetSticky(hookObject.hookHitAudio.clip.length);

        return hookObject;
    }

    public void PlayHitAudio()
    {
        hookHitAudio.Play();
    }
}
