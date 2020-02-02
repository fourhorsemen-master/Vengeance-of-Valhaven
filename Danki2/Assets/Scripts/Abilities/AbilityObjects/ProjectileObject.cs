using System;
using System.Collections;
using UnityEngine;

public abstract class ProjectileObject : MonoBehaviour
{
    private Actor _caster;
    private float _speed;
    private Action<GameObject> _collisionCallback;
    private bool _isSticky = false;
    private float _stickTime = 0f;

    /// <summary>
    /// To be called after instantiation. Subclasses will have their own Initialise methods with extra paramaters which will call this first.
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="collisionCallback"></param>
    /// <param name="speed"></param>
    protected ProjectileObject InitialiseProjectile(Actor caster, Action<GameObject> collisionCallback, float speed)
    {
        _caster = caster;
        _collisionCallback = collisionCallback;
        _speed = speed;

        return this;
    }

    public void SetSticky(float stickTime)
    {
        _isSticky = true;
        _stickTime = stickTime;
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (GameObject.ReferenceEquals(_caster.gameObject, other.gameObject)) return;

        Debug.Log(other.gameObject.name.ToString());

        _collisionCallback(other.gameObject);

        if (!_isSticky)
        {
            Destroy(gameObject);
            return;
        }

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        Collider coll = gameObject.GetComponent<Collider>();
        Destroy(coll);
        transform.SetParent(other.transform);
        _speed = 0f;

        StartCoroutine(DissapearAfter(_stickTime));
    }

    IEnumerator DissapearAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
