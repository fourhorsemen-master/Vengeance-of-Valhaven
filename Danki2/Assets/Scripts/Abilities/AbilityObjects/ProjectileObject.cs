using System;
using UnityEngine;

public abstract class ProjectileObject : MonoBehaviour
{
    protected Actor caster;
    private float horizontalSpeed;
    private float verticalSpeed;
    private Action<GameObject> collisionCallback;
    private bool gravitationallyAffected = false;
    private bool isSticky = false;
    private float stickTime = 0f;

    /// <summary>
    /// To be called after instantiation. Subclasses will have their own Initialise methods with extra paramaters which will call this first.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="collisionCallback"></param>
    /// <param name="horizontalSpeed"></param>
    /// <param name="verticalSpeed">Initial vertical speed, affected by gravity.</param>
    protected ProjectileObject InitialiseProjectile(Actor caster, Action<GameObject> collisionCallback, float horizontalSpeed, float verticalSpeed = float.NaN)
    {
        this.caster = caster;
        this.collisionCallback = collisionCallback;
        this.horizontalSpeed = horizontalSpeed;        

        if (!float.IsNaN(verticalSpeed))
        {
            this.verticalSpeed = verticalSpeed;
            this.gravitationallyAffected = true;
        }

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

    public void StopProjectile()
    {
        this.horizontalSpeed = 0f;
        this.verticalSpeed = 0f;
        this.gravitationallyAffected = false;
    }

    private void Update()
    {
        transform.position += transform.forward * this.horizontalSpeed * Time.deltaTime;

        if(gravitationallyAffected)
        {
            transform.position += transform.up * this.verticalSpeed * Time.deltaTime;
            this.verticalSpeed -= Physics.gravity.magnitude * Time.deltaTime;
        }        
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
        this.horizontalSpeed = 0f;
        this.verticalSpeed = 0f;
        this.gravitationallyAffected = false;

        this.WaitAndAct(this.stickTime, () => Destroy(gameObject));
    }
}
