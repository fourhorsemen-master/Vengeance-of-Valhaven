using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class SupplementaryTooltipPanel : MonoBehaviour
{
    [SerializeField]
    private float displayDelay = 0f;

    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private RectTransform tooltipRectTransform = null;

    [SerializeField]
    private SupplementaryTooltip supplementaryTooltipPrefab = null;

    private Dictionary<ScreenQuadrant, Vector2> pivotPoints = new Dictionary<ScreenQuadrant, Vector2>
    {
        { ScreenQuadrant.TopLeft, new Vector2(0, 0) },
        { ScreenQuadrant.TopRight, new Vector2(1, 0) },
        { ScreenQuadrant.BottomLeft, new Vector2(0, 1) },
        { ScreenQuadrant.BottomRight, new Vector2(1, 1) }
    };

    private List<ScreenQuadrant> leftQuadrants = new List<ScreenQuadrant> {
        ScreenQuadrant.TopLeft,
        ScreenQuadrant.BottomLeft
    };

    private ScreenQuadrant currentScreenQuadrant;
    private Coroutine displayCoroutine;

    public void OnDestroy()
    {
        StopCoroutine(displayCoroutine);
    }

    private void Update()
    {
        bool isLeftQuadrant = leftQuadrants.Contains(currentScreenQuadrant);
        float horizontalOffset = tooltipRectTransform.sizeDelta.x * tooltipRectTransform.GetParentCanvas().scaleFactor * (isLeftQuadrant ? 1 : -1);
        transform.position = tooltipRectTransform.position + new Vector3(horizontalOffset, 0);
    }

    public void Activate(AbilityReference ability)
    {
        Activate();

        displayCoroutine = this.WaitAndAct(displayDelay, () => Display(KeywordUtils.GetKeywords(ability)));
    }

    public void Activate(Rune rune)
    {
        Activate();

        displayCoroutine = this.WaitAndAct(displayDelay, () => Display(KeywordUtils.GetKeywords(rune)));
    }

    private void Activate()
    {
        currentScreenQuadrant = InputHelpers.GetMouseScreenQuadrant();
        rectTransform.pivot = pivotPoints[currentScreenQuadrant];
    }

    private void Display(List<Keyword> keywords)
    {
        keywords.ForEach(k =>
        {
            Instantiate(supplementaryTooltipPrefab, transform)
                .Setup(k);
        });
    }
}
