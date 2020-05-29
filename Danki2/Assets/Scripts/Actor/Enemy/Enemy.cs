using Assets.Scripts.UnityHelpers.Extensions;
using System;
using UnityEngine;

public abstract class Enemy : Actor
{
    public static readonly Color HighlightedColor = new Color(0.02f, 0.02f, 0.02f);

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    public Subject<float> OnTelegraph { get; private set; } = new Subject<float>();

    protected virtual void Start()
    {
        this.gameObject.tag = Tags.Enemy;
    }

    public void WaitAndCast(float waitTime, AbilityReference abilityReference, Func<Vector3> targeter)
    {
        OnTelegraph.Next(waitTime);

        MovementManager.Stun(waitTime);

        InterruptableAction(waitTime, InterruptionType.Hard, () =>
        {
            InstantCastService.Cast(abilityReference, targeter());
        });
    }

    public void SetHighlighted(bool highlighted)
    {
        Color colour = highlighted ? HighlightedColor : Color.clear;

        meshRenderer.material.SetEmissiveColour(colour);
    }
}
