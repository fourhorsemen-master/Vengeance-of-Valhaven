using UnityEngine;

public class FireballObject : MonoBehaviour
{
    private static readonly float SPEED = 5;

    private Vector3 _direction;

    public static void Fire(Vector3 position, Vector3 direction)
    {
        FireballObject prefab = AbilityObjectLookup.Instance.FireballObjectPrefab;
        FireballObject fireballObject = Instantiate(prefab, position, default);
        direction.Normalize();
        fireballObject._direction = direction;
    }

    private void Update()
    {
        transform.position += _direction * Time.deltaTime * SPEED;
    }
}
