using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class AbilitySupplementaryTooltipPanel : MonoBehaviour
{
    [SerializeField]
    private float displayDelay = 0f;

    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Transform followTransform = null;

    [SerializeField]
    private RectTransform abilityTooltipRectTransform = null;

    [SerializeField]
    private AbilitySupplementaryTooltip abilitySupplementaryTooltipPrefab = null;

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
        float horizontalOffset = abilityTooltipRectTransform.sizeDelta.x * abilityTooltipRectTransform.GetParentCanvas().scaleFactor * (isLeftQuadrant ? 1 : -1);
        transform.position = followTransform.position + new Vector3(horizontalOffset, 0);
    }

    public void Activate(AbilityReference ability)
    {
        currentScreenQuadrant = InputHelpers.GetMouseScreenQuadrant();
        rectTransform.pivot = pivotPoints[currentScreenQuadrant];

        displayCoroutine = this.WaitAndAct(displayDelay, () => Display(ability));
    }

    private void Display(AbilityReference ability)
    {
        var keywords = KeywordUtils.GetKeywords(ability);

        keywords.ForEach(k =>
        {
            Instantiate(abilitySupplementaryTooltipPrefab, transform)
                .Setup(k);
        });
    }
}
