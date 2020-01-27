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
        if (!other.GetComponent<Player>())
        {
            transform.SetParent(other.transform);
            _speed = 0f;
        }
        base.OnTriggerEnter(other);
        StartCoroutine(Dissapear());
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
