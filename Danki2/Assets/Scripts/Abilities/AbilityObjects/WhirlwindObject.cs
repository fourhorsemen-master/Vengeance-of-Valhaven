using UnityEngine;
using UnityEngine.VFX;

public class WhirlwindObject : MonoBehaviour
{
    private const float particleDissapationPeriod = 0.4f;

    private VisualEffect pfx;

    public static WhirlwindObject Create(Transform casterTransform)
    {
        WhirlwindObject prefab = AbilityObjectPrefabLookup.Instance.WhirlwindObjectPrefab;
        return Instantiate(prefab, casterTransform);
    }

    public void DissipateAndDestroy()
    {
        pfx.Stop();
        Destroy(gameObject, particleDissapationPeriod);
    }

    public void Start()
    {
        pfx = gameObject.GetComponent<VisualEffect>();
    }
}
