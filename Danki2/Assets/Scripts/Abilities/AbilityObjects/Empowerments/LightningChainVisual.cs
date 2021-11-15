using UnityEngine;
using UnityEngine.VFX;

public class LightningChainVisual : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect = null;

    public static void Create(Vector3 startPosition, Vector3 endPosition)
    {
        LightningChainVisual prefab = AbilityObjectPrefabLookup.Instance.LightningChainVisualPrefab;
        LightningChainVisual lightningChainVisual = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        lightningChainVisual.visualEffect.SetVector3("Start", startPosition);
        lightningChainVisual.visualEffect.SetVector3("End", endPosition);
        lightningChainVisual.visualEffect.Play();
    }


}
