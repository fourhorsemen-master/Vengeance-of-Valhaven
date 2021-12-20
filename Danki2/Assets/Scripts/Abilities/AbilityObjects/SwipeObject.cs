using UnityEngine;

public class SwipeObject : AbilityObject
{
    public static SwipeObject Create(Vector3 position, Quaternion rotation)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.SwipeObjectPrefab, position, rotation);
    }
}