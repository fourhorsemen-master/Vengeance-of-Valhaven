using UnityEngine;

public class CleaveObject : MonoBehaviour
{    public static CleaveObject Create(Vector3 position, Quaternion rotation)
    {
        CleaveObject prefab = AbilityObjectPrefabLookup.Instance.CleaveObjectPrefab;
        return Instantiate(prefab, position, rotation);
    }
}
