using System;
using UnityEngine;

public class HighlightManager
{
    private readonly Renderer[] meshRenderers;

    private float currentIntensity = 0;

    private readonly Registry<float> intensities;

    public HighlightManager(Subject updateSubject, Renderer[] meshRenderers)
    {
        this.meshRenderers = meshRenderers;

        intensities = new Registry<float>(updateSubject);

        updateSubject.Subscribe(ApplyCurrentHighlight);
    }

    public Guid AddIndefiniteHighlight(float intensity) => intensities.AddIndefinite(intensity);

    public Guid AddTemporaryHighlight(float intensity, float duration) => intensities.AddTemporary(intensity, duration);

    public void RemoveHighlight(Guid id) => intensities.Remove(id);

    private void ApplyCurrentHighlight()
    {
        float nextIntensity = 0f;

        intensities.ForEach(i => {
            nextIntensity = Mathf.Max(nextIntensity, i);
        });

        if (currentIntensity == nextIntensity) return;

        currentIntensity = nextIntensity;
        Color highlight = new Color(nextIntensity, nextIntensity, nextIntensity);

        foreach (Renderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetEmissiveColour(highlight);
        }
    }
}
