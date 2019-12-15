using System;
using UnityEngine;

public class FireballObject : MonoBehaviour
{
    private float _speed;
    private Actor _caster;
    private Action<GameObject> _collisionCallback;

    public static void Fire(Actor caster, Action<GameObject> collisionCallback, Vector3 position, Quaternion rotation, float speed)
    {
        FireballObject prefab = AbilityObjectPrefabLookup.Instance.FireballObjectPrefab;
        FireballObject fireballObject = Instantiate(prefab, position, rotation);
        fireballObject._speed = speed;
        fireballObject._caster = caster;
        fireballObject._collisionCallback = collisionCallback;
    }

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
