using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmissiveManager
{
    private Dictionary<Material, Emissive> materialsToEmissive;
    private readonly Actor actor;

    private const float HighlightIntensity = 0.2f;
    private const float FlashIntensity = 0.3f;
    private const float FlashDuration = 0.1f;

    private Coroutine cancelFlashCoroutine = null;

    private bool isFlashing = false;
    private bool isHighlighted = false;

    public EmissiveManager(Actor actor, Subject startSubject, Renderer[] renderers)
    {
        this.actor = actor;

        startSubject.Subscribe(() => {
            materialsToEmissive = renderers.ToDictionary(
                x => x.material,
                x => Emissive.FromMaterial(x.material)
            );
        });
    }

    public void StartHighlight()
    {
        isHighlighted = true;
        UpdateEmissive();
    }

    public void StopHighlight()
    {
        isHighlighted = false;
        UpdateEmissive();
    }

    public void Flash()
    {
        if (cancelFlashCoroutine != null) actor.StopCoroutine(cancelFlashCoroutine);

        isFlashing = true;
        UpdateEmissive();

        cancelFlashCoroutine = actor.WaitAndAct(FlashDuration, () =>
        {
            isFlashing = false;
            UpdateEmissive();
        });
    }

    private void UpdateEmissive()
    {
        materialsToEmissive.Keys
            .ToList()
            .ForEach(material =>
            {
                Emissive emissive = materialsToEmissive[material];

                Texture map = isFlashing
                    ? Texture2D.whiteTexture
                    : isHighlighted
                        ? emissive.HighlightedMap
                        : emissive.BaseMap;

                Color colour = isFlashing
                    ? Color.white * FlashIntensity
                    : emissive.Colour;

                material.SetEmissiveMap(map);
                material.SetEmissiveColour(colour);
            });
    }

    private static Texture2D BuildHighlightedMap(Texture baseMap)
    {
        Texture2D highlightedMap;

        if (baseMap == null)
        {
            highlightedMap = Texture2D.blackTexture;
            highlightedMap.SetPixels(highlightedMap.GetPixels().Select(x => Color.Lerp(x, Color.white, HighlightIntensity)).ToArray());
            return highlightedMap;
        }

        highlightedMap = new Texture2D(baseMap.width, baseMap.height);
        highlightedMap.SetPixels(((Texture2D)baseMap).GetPixels().Select(x => Color.Lerp(x, Color.white, HighlightIntensity)).ToArray());
        highlightedMap.Apply();

        return highlightedMap;
    }

    private class Emissive
    {
        public static Emissive FromMaterial(Material material) =>
            new Emissive
            {
                BaseMap = material.GetEmissiveMap(),
                HighlightedMap = BuildHighlightedMap(material.GetEmissiveMap()),
                Colour = material.GetEmissiveColour()
            };

        public Texture BaseMap { get; private set; }
        public Texture HighlightedMap { get; private set; }
        public Color Colour { get; private set; }
    }
}
