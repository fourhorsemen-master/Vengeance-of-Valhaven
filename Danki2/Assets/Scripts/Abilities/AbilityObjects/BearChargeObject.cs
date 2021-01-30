using UnityEngine;

public class BearChargeObject : MonoBehaviour
{
    public static BearChargeObject Create(Transform transform)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.BearChargeObjectPrefab, transform);
    }

    public void CreateSwipe(Vector3 position, Quaternion rotation, bool playHitSound)
    {
        SwipeObject swipeObject = SwipeObject.Create(position, rotation);
        if (playHitSound) swipeObject.PlayHitSound();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
