using System;
using System.Linq;
using UnityEngine;

public class HighlightManager
{
    private readonly MeshRenderer meshRenderer;

    private float currentIntensity = 0;

    private Registry<float> intensities;

    public HighlightManager(Subject updateSubject, MeshRenderer meshRenderer)
    {
        this.meshRenderer = meshRenderer;

        intensities = new Registry<float>(updateSubject);

        updateSubject.Subscribe(ApplyCurrentHighlight);
    }

    public Guid AddIndefiniteHighlight(float intensity) => intensities.AddIndefinite(intensity);

    public Guid AddTemporaryHighlight(float intensity, float duration) => intensities.AddTemporary(intensity, duration);

    public void RemoveHightlight(Guid id) => intensities.Remove(id);

    private void ApplyCurrentHighlight()
    {
        var nexIntensity = intensities.Entities.Any() ? intensities.Entities.Values.Max() : 0f;

        if (currentIntensity == nexIntensity) return;

        currentIntensity = nexIntensity;
        Color highlight = new Color(nexIntensity, nexIntensity, nexIntensity);
        meshRenderer.material.SetEmissiveColour(highlight);
    }
}