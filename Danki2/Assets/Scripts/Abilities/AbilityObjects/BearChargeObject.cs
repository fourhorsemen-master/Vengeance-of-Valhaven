using UnityEngine;

public class BearChargeObject : AbilityObject
{
    public static BearChargeObject Create(Transform transform)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.BearChargeObjectPrefab, transform);
    }

    public void CreateSwipe(Vector3 position, Quaternion rotation, bool playHitSound)
    {
        SwipeObject.Create(position, rotation);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
