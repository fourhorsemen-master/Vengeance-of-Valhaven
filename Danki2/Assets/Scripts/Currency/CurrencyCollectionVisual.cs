using System;
using UnityEngine;
using UnityEngine.VFX;

public class CurrencyCollectionVisual : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect;

    private Player player = null;

    private int currencyAmount = 0;

    private void Start()
    {
        player = ActorCache.Instance.Player;

        this.ActOnInterval(0.1f, _ => visualEffect.SetVector3("Target", player.Centre));

        visualEffect.SetInt("SpawnCount", currencyAmount);

        visualEffect.Play();
    }

    public static void Create(Vector3 position, int currencyAmount)
    {
        Instantiate(CurrencyLookup.Instance.CurrencyCollectionVisualPrefab, position, Quaternion.identity)
            .currencyAmount = currencyAmount;
    }
}
