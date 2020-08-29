using System;
using UnityEngine;

public abstract class ProjectileObject : MonoBehaviour
{
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

        return this;
    }

    public void SetSticky(float stickTime)
    {
        this.isSticky = true;
        this.stickTime = stickTime;
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
        transform.position += transform.forward * this.speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (ReferenceEquals(this.caster.gameObject, other.gameObject)) return;

        this.collisionCallback(other.gameObject);

        if (!this.isSticky)
        {
            Destroy(gameObject);
            return;
        }

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        Collider coll = gameObject.GetComponent<Collider>();
        Destroy(coll);
        transform.SetParent(other.transform);
        this.speed = 0f;

        this.WaitAndAct(this.stickTime, () => Destroy(gameObject));
    }
}
