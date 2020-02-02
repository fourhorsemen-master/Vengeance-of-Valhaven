using System;
using UnityEngine;
using System.Collections;

public class DaggerObject : ProjectileObject
{
    public static void Fire(Actor caster, Action<GameObject> collisionCallback, float speed, Vector3 position, Quaternion rotation)
    {
        var prefab = AbilityObjectPrefabLookup.Instance.DaggerObjectPrefab;
        Instantiate(prefab, position, rotation)
            .InitialiseProjectile(caster, collisionCallback, speed);
    }

    override protected void OnTriggerEnter(Collider other)
    {
        if (!GameObject.ReferenceEquals(_caster.gameObject, other.gameObject))
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            Destroy(rb);
            transform.SetParent(other.transform);
            _speed = 0f;
        }
        
        if (other.gameObject == _caster.gameObject) return;

        _collisionCallback(other.gameObject);
        StartCoroutine(Dissapear());
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
