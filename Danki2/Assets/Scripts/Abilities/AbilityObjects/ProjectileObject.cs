using System;
using UnityEngine;

public abstract class ProjectileObject : MonoBehaviour
{
    protected Actor _caster;
    protected float _speed;
    protected Action<GameObject> _collisionCallback;

    /// <summary>
    /// To be called after instantiation. Subclasses will have their own Initialise methods with extra paramaters which will call this first.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="collisionCallback"></param>
    /// <param name="speed"></param>
    protected void InitialiseProjectile(Actor caster, Action<GameObject> collisionCallback, float speed)
    {
        _caster = caster;
        _collisionCallback = collisionCallback;
        _speed = speed;
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _caster.gameObject) return;

        _collisionCallback(other.gameObject);
    }
}
