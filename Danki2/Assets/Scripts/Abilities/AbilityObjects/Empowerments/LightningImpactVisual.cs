using UnityEngine;
using UnityEngine.VFX;

public class LightningImpactVisual : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect = null;

    public static void Create(Vector3 position)
    {
        LightningImpactVisual prefab = AbilityObjectPrefabLookup.Instance.LightningImpactVisualPrefab;
        LightningImpactVisual lightningImpactVisual = Instantiate(prefab, position, Quaternion.identity);
        lightningImpactVisual.visualEffect.Play();
    }


}
