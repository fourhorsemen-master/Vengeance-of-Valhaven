using UnityEngine;

public class FireballObject : MonoBehaviour
{
    private float _speed;
    private Actor _caster;

    public static void Fire(Actor caster, Vector3 position, Quaternion rotation, float speed)
    {
        FireballObject prefab = AbilityObjectPrefabLookup.Instance.FireballObjectPrefab;
        FireballObject fireballObject = Instantiate(prefab, position, rotation);
        fireballObject._speed = speed;
        fireballObject._caster = caster;
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != _caster.gameObject) Destroy(gameObject);
    }
}
