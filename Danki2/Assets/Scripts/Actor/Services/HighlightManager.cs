using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighlightManager
{
    private readonly MeshRenderer meshRenderer;

    private readonly Dictionary<Guid, float> highlights = new Dictionary<Guid, float>();

    private readonly Dictionary<Guid, float> durations = new Dictionary<Guid, float>();

    private float currentIntensity = 0;

    public HighlightManager(Subject updateSubject, MeshRenderer meshRenderer)
    {
        this.meshRenderer = meshRenderer;

        updateSubject.Subscribe(ApplyCurrentHighlight);
    }

    public Guid AddIndefiniteHighlight(float intensity) => AddHighlight(intensity);

    public Guid AddTemporaryHighlight(float intensity, float duration)
    {
        Guid id = AddHighlight(intensity);

        durations.Add(id, duration);

        return id;
    }

    public void RemoveHightlight(Guid id)
    {
        // Note that Dicionary.Remove returns false if the key is not found.
        highlights.Remove(id);
        durations.Remove(id);
    }

    private Guid AddHighlight(float intensity)
    {
        Guid id = Guid.NewGuid();

        highlights.Add(id, intensity);

        return id;
    }

    private void ApplyCurrentHighlight()
    {
        TickDurations();

        var nexIntensity = highlights.Any() ? highlights.Values.Max() : 0f;

        if (currentIntensity == nexIntensity) return;

        currentIntensity = nexIntensity;
        Color highlight = new Color(nexIntensity, nexIntensity, nexIntensity);
        meshRenderer.material.SetEmissiveColour(highlight);
    }

    private void TickDurations()
    {
        durations.Keys.ToList().ForEach(id =>
        {
            durations[id] -= Time.deltaTime;

            if (durations[id] <= 0)
            {
                RemoveHightlight(id);
            }
        });
    }
}