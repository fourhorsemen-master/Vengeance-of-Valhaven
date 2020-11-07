using System;
using System.Linq;
using UnityEngine;

public class HighlightManager
{
    private readonly MeshRenderer meshRenderer;

    private float currentIntensity = 0;

    private Registry<float> registry;

    public HighlightManager(Subject updateSubject, MeshRenderer meshRenderer)
    {
        this.meshRenderer = meshRenderer;

        registry = new Registry<float>(updateSubject);

        updateSubject.Subscribe(ApplyCurrentHighlight);
    }

    public Guid AddIndefiniteHighlight(float intensity) => registry.AddIndefinite(intensity);

    public Guid AddTemporaryHighlight(float intensity, float duration) => registry.AddTemporary(intensity, duration);

    public void RemoveHightlight(Guid id) => registry.Remove(id);

    private void ApplyCurrentHighlight()
    {
        var nexIntensity = registry.Entities.Any() ? registry.Entities.Values.Max() : 0f;

        if (currentIntensity == nexIntensity) return;

        currentIntensity = nexIntensity;
        Color highlight = new Color(nexIntensity, nexIntensity, nexIntensity);
        meshRenderer.material.SetEmissiveColour(highlight);
    }
}