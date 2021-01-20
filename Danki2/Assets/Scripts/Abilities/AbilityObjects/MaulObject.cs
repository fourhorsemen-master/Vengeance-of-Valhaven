using UnityEngine;

public class MaulObject : MonoBehaviour
{
    public static void Create(Vector3 position)
    {
        var maulObjectPrefab = AbilityObjectPrefabLookup.Instance.MaulObjectPrefab;
        Instantiate(maulObjectPrefab, position, Quaternion.identity);
    }
}
