using UnityEngine;

public class FireballObject : MonoBehaviour
{
    private static readonly float SPEED = 5;

    public static void Fire(Vector3 position, Quaternion rotation)
    {
        FireballObject prefab = AbilityObjectLookup.Instance.FireballObjectPrefab;
        Instantiate(prefab, position, rotation);
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * SPEED;
    }
}
