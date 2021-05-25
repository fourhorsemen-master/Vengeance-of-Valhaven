using System;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager
{
    private readonly Registry<float> intensities;

    private readonly Dictionary<Material, Color> materialToInitialEmissiveColour;

    public HighlightManager(Subject updateSubject, Renderer[] renderers)
    {
        intensities = new Registry<float>(updateSubject, UpdateHighlight, UpdateHighlight);

        materialToInitialEmissiveColour = new Dictionary<Material, Color>();

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                materialToInitialEmissiveColour[material] = material.GetEmissiveColour();
            }
        }
    }

    public Guid AddIndefiniteHighlight(float intensity) => intensities.AddIndefinite(intensity);

    public Guid AddTemporaryHighlight(float intensity, float duration) => intensities.AddTemporary(intensity, duration);

    public void RemoveHighlight(Guid id) => intensities.Remove(id);

    private void UpdateHighlight(Guid _, float __)
    {
        float nextIntensity = 0f;
        intensities.ForEach(i => nextIntensity = Mathf.Max(nextIntensity, i));
        Color highlight = new Color(nextIntensity, nextIntensity, nextIntensity);

        foreach (KeyValuePair<Material,Color> kvp in materialToInitialEmissiveColour)
        {
            kvp.Key.SetEmissiveColour(highlight + kvp.Value);
        }
    }
}
