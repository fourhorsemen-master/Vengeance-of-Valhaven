using UnityEngine;
using UnityEngine.VFX;

public class LightningVisual : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect = null;

    public static void Create(Vector3 startPosition, Vector3 endPosition)
    {
        LightningVisual prefab = AbilityObjectPrefabLookup.Instance.LightningVisualPrefab;
        LightningVisual lightningVisual = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        lightningVisual.visualEffect.SetVector3("Start", startPosition);
        lightningVisual.visualEffect.SetVector3("End", endPosition);
        lightningVisual.visualEffect.Play();
    }


}
