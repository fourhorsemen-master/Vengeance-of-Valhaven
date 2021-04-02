using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileObject : AbilityObject
{
    // Need the new keyword here to show we are overriding the deprecated base collider property.
    [SerializeField]
    new Collider collider = null;

    [SerializeField]
    Rigidbody rigidBody = null;

    private const float MaxDistanceFromPlayer = 200f;

    protected Actor caster;
    private float speed;
    private Action<GameObject> collisionCallback;
    private bool isSticky = false;
    private float stickTime = 0f;

    /// <summary>
    /// To be called after instantiation. Subclasses will have their own Initialise methods with extra paramaters which will call this first.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="collisionCallback"></param>
    /// <param name="speed"></param>
    protected ProjectileObject InitialiseProjectile(Actor caster, Action<GameObject> collisionCallback, float speed)
    {
        this.caster = caster;
        this.collisionCallback = collisionCallback;
        this.speed = speed;

        // Every second, destroy projectile if beyond certain distance from player
        this.ActOnInterval(1, _ => {
            if (transform.DistanceFromPlayer() > MaxDistanceFromPlayer)
            {
                Destroy(gameObject);
            }
        });

        return this;
    }

    /// <summary>
    /// Set a time after which the object is destroyed.
    /// </summary>
    public void SetSticky(float duration)
    {
        isSticky = true;
        stickTime = duration;
    }

    public void DestroyAfterTime(float timePeriod, Action callback = null)
    {
        this.WaitAndAct(timePeriod, () =>
        {
            callback?.Invoke();
            Destroy(gameObject);
        });
    }

    protected virtual void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (ReferenceEquals(caster.gameObject, other.gameObject)) return;

        collisionCallback(other.gameObject);

        if (other.sharedMaterial != null)
        {
            CollisionSoundManager.Instance.Play(other.sharedMaterial, CollisionSoundLevel.Low);
        }

        if (isSticky)
        {
            StickTo(other.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void StickTo(Transform newParent)
    {
        Destroy(rigidBody);
        Destroy(collider);
        transform.SetParent(newParent);
        speed = 0f;

        this.WaitAndAct(stickTime, () => Destroy(gameObject));
    }
}
