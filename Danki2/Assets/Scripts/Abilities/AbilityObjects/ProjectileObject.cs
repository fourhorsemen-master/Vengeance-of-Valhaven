using System;
using UnityEngine;

public abstract class ProjectileObject : MonoBehaviour
{
    protected Actor _caster;
    protected float _speed;
    protected Action<GameObject> _collisionCallback;

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _caster.gameObject) return;

        _collisionCallback(other.gameObject);

        Destroy(gameObject);
    }
}
