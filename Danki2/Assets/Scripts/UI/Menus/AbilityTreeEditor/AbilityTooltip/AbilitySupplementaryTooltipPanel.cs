using System.Collections.Generic;
using UnityEngine;

public class AbilitySupplementaryTooltipPanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private AbilityTooltip abilityTooltip = null;

    [SerializeField]
    private AbilitySupplementaryTooltip abilitySupplementaryTooltipPrefab = null;

    private Dictionary<ScreenQuadrant, Vector2> pivotPoints = new Dictionary<ScreenQuadrant, Vector2>
    {
        { ScreenQuadrant.TopLeft, new Vector2(0, 0) },
        { ScreenQuadrant.TopRight, new Vector2(1, 0) },
        { ScreenQuadrant.BottomLeft, new Vector2(0, 1) },
        { ScreenQuadrant.BottomRight, new Vector2(1, 1) }
    };

    private void Start()
    {
        abilityTooltip.OnActivate.Subscribe(Activate);
        abilityTooltip.OnDeactivate.Subscribe(Deactivate);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = abilityTooltip.transform.position;
    }

    private void Activate(AbilityReference ability)
    {
        gameObject.SetActive(true);

        ScreenQuadrant currentScreenQuadrant = InputHelpers.GetMouseScreenQuadrant();
        rectTransform.pivot = pivotPoints[currentScreenQuadrant];

        var tooltipTypes = SupplementaryTooltipUtils.GetSupplementaryTooltips(ability);

        tooltipTypes.ForEach(t =>
        {
            Instantiate(abilitySupplementaryTooltipPrefab, transform)
            .Setup(t);
        });
    }

    private void Deactivate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        gameObject.SetActive(false);
    }
}
