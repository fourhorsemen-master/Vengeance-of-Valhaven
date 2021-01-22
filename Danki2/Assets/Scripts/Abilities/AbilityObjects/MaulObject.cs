using System;
using UnityEngine;

public class MaulObject : MonoBehaviour
{
    public static MaulObject Create(Vector3 position)
    {
        var maulObjectPrefab = AbilityObjectPrefabLookup.Instance.MaulObjectPrefab;
        return Instantiate(maulObjectPrefab, position, Quaternion.identity);
    }

    public void Bite(Quaternion castRotation)
    {
        BiteObject prefab = AbilityObjectPrefabLookup.Instance.BiteObjectPrefab;
        BiteObject biteObject = Instantiate(prefab, transform);
        biteObject.transform.rotation = castRotation;
    }
}
