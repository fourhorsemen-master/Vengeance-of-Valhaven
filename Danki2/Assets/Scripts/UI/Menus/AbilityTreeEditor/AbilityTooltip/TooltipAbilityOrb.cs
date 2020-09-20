﻿using UnityEngine;
using UnityEngine.UI;

public class TooltipAbilityOrb : MonoBehaviour
{
    [SerializeField]
    private Color highlightedColour = Color.white;

    [SerializeField]
    private Color unhighlightedColour = Color.grey;

    [SerializeField]
    private Image image = null;

    public void SetType(OrbType orbType, bool highlighted)
    {
        Sprite sprite = OrbLookup.Instance.GetSprite(orbType);
        image.sprite = sprite;
        image.color = highlighted ? highlightedColour : unhighlightedColour;
    }
}