using UnityEngine;

public class ParryObject : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("start of parry object...");
    }

    public static ParryObject Create(Transform transform)
    {
        ParryObject prefab = AbilityObjectPrefabLookup.Instance.ParryObjectPrefab;
        return Instantiate(prefab, transform);
    }
}
