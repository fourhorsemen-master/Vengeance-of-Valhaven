using UnityEngine;

public class FireballObject : MonoBehaviour
{
    private float _speed;

    public static void Fire(Vector3 position, Quaternion rotation, float speed)
    {
        FireballObject prefab = AbilityObjectPrefabLookup.Instance.FireballObjectPrefab;
        FireballObject fireballObject = Instantiate(prefab, position, rotation);
        fireballObject._speed = speed;
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }
}
