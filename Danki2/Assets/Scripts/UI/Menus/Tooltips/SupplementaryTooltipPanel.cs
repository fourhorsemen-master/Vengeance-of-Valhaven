using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class SupplementaryTooltipPanel : MonoBehaviour
{
    [SerializeField]
    public float displayDelay = 0f;

    [SerializeField]
    public RectTransform rectTransform = null;

    [SerializeField]
    public RectTransform tooltipRectTransform = null;

    [SerializeField]
    public SupplementaryTooltip supplementaryTooltipPrefab = null;

    private readonly Dictionary<ScreenQuadrant, Vector2> pivotPoints = new Dictionary<ScreenQuadrant, Vector2>
    {
        { ScreenQuadrant.TopLeft, new Vector2(0, 0) },
        { ScreenQuadrant.TopRight, new Vector2(1, 0) },
        { ScreenQuadrant.BottomLeft, new Vector2(0, 1) },
        { ScreenQuadrant.BottomRight, new Vector2(1, 1) }
    };

    private readonly List<ScreenQuadrant> leftQuadrants = new List<ScreenQuadrant> {
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

    public void Activate(
        List<Empowerment> empowerments = null,
        List<ActiveEffect> activeEffects = null,
        List<PassiveEffect> passiveEffects = null,
        List<StackingEffect> stackingEffects = null
    )
    {
        Activate();

        List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
        empowerments?.Distinct().ToList().ForEach(e => tuples.Add(Tuple.Create(
            EmpowermentLookup.Instance.GetDisplayName(e),
            EmpowermentLookup.Instance.GetTooltip(e)
        )));
        activeEffects?.Distinct().ToList().ForEach(e => tuples.Add(Tuple.Create(
            EffectLookup.Instance.GetDisplayName(e),
            EffectLookup.Instance.GetTooltip(e)
        )));
        passiveEffects?.Distinct().ToList().ForEach(e => tuples.Add(Tuple.Create(
            EffectLookup.Instance.GetDisplayName(e),
            EffectLookup.Instance.GetTooltip(e)
        )));
        stackingEffects?.Distinct().ToList().ForEach(e => tuples.Add(Tuple.Create(
            EffectLookup.Instance.GetDisplayName(e),
            EffectLookup.Instance.GetTooltip(e)
        )));
        displayCoroutine = this.WaitAndAct(displayDelay, () => Display(tuples));
    }

    private void Activate()
    {
        currentScreenQuadrant = InputHelpers.GetMouseScreenQuadrant();
        rectTransform.pivot = pivotPoints[currentScreenQuadrant];
    }

    private void Display(List<Tuple<string, string>> tooltips)
    {
        tooltips.ForEach(t =>
        {
            Instantiate(supplementaryTooltipPrefab, transform)
                .Setup(t.Item1, t.Item2);
        });
    }
}
