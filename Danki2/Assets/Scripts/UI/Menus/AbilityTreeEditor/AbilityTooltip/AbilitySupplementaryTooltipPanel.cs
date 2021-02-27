using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class AbilitySupplementaryTooltipPanel : MonoBehaviour
{
    [SerializeField]
    private float displayDelay;

    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private AbilityTooltip abilityTooltip = null;

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

    private void Start()
    {
        abilityTooltip.OnActivate.Subscribe(Activate);
        abilityTooltip.OnDeactivate.Subscribe(Deactivate);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        bool isLeftQuadrant = leftQuadrants.Contains(currentScreenQuadrant);
        float horizontalOffset = abilityTooltipRectTransform.sizeDelta.x * abilityTooltipRectTransform.GetParentCanvas().scaleFactor * (isLeftQuadrant ? 1 : -1);
        transform.position = abilityTooltip.transform.position + new Vector3(horizontalOffset, 0);
    }

    private void Activate(AbilityReference ability)
    {
        gameObject.SetActive(true);

        currentScreenQuadrant = InputHelpers.GetMouseScreenQuadrant();
        rectTransform.pivot = pivotPoints[currentScreenQuadrant];

        displayCoroutine = this.WaitAndAct(displayDelay, () => Display(ability));
    }

    private void Display(AbilityReference ability)
    {
        var keywords = KeywordUtils.GetKeywords(ability);

        keywords.ForEach(t =>
        {
            Instantiate(abilitySupplementaryTooltipPrefab, transform)
                .Setup(t);
        });
    }

    private void Deactivate()
    {
        StopCoroutine(displayCoroutine);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        gameObject.SetActive(false);
    }
}
