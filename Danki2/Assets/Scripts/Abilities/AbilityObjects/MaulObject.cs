using UnityEngine;

public class MaulObject : AbilityObject
{
    private const float ForwardOffset = 1f;

    public static MaulObject Create(Vector3 position)
    {
        MaulObject maulObjectPrefab = AbilityObjectPrefabLookup.Instance.MaulObjectPrefab;
        return Instantiate(maulObjectPrefab, position, Quaternion.identity);
    }

    public void Bite(Quaternion castRotation)
    {
        BiteObject prefab = AbilityObjectPrefabLookup.Instance.BiteObjectPrefab;
        BiteObject biteObject = Instantiate(prefab, transform);
        biteObject.transform.rotation = castRotation;
        biteObject.transform.position += biteObject.transform.forward * ForwardOffset;
    }
}
