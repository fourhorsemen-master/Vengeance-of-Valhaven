using UnityEngine;
using UnityEngine.VFX;

public class CurrencyCollectionVisual : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect;

    [SerializeField]
    private float targetRefreshInterval = 0f;

    private void Start()
    {
        this.ActOnInterval(
            targetRefreshInterval,
            _ => visualEffect.SetVector3("Target", ActorCache.Instance.Player.Centre)
        );
    }

    public static void Create(Vector3 position, int currencyAmount)
    {
        VisualEffect vfx = Instantiate(CurrencyLookup.Instance.CurrencyCollectionVisualPrefab, position, Quaternion.identity).visualEffect;
        vfx.SetInt("SpawnCount", currencyAmount);
        vfx.SetVector4("Colour", VisualSettings.Instance.EnergyColour);
    }
}
