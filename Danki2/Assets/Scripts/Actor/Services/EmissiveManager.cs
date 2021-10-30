using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmissiveManager
{
    private Dictionary<Material, Texture> materialsToEmissiveMaps;
    private Dictionary<Material, Color> materialsToEmissiveColours;
    private readonly Actor actor;

    private const float FlashIntensity = 0.3f;
    private const float FlashDuration = 0.1f;

    private Coroutine cancelFlashCoroutine = null;

    public EmissiveManager(Actor actor, Subject startSubject, Renderer[] renderers)
    {
        this.actor = actor;

        startSubject.Subscribe(() => {
            materialsToEmissiveMaps = renderers.ToDictionary(
                x => x.material,
                x => x.material.GetEmissiveMap()
            );
            materialsToEmissiveColours = renderers.ToDictionary(
                x => x.material,
                x => x.material.GetEmissiveColour()
            );
        });
    }

    public void Flash()
    {
        if (cancelFlashCoroutine != null) actor.StopCoroutine(cancelFlashCoroutine);

        SetFlashEmissive();

        cancelFlashCoroutine = actor.WaitAndAct(FlashDuration, ResetEmissive);
    }

    private void SetFlashEmissive()
    {
        materialsToEmissiveMaps.Keys.ToList()
            .ForEach(x =>
            {
                x.SetEmissiveMap(Texture2D.whiteTexture);
                x.SetEmissiveColour(Color.white * FlashIntensity);
            });
    }

    private void ResetEmissive()
    {
        materialsToEmissiveMaps.Keys.ToList()
            .ForEach(x =>
            {
                x.SetEmissiveMap(materialsToEmissiveMaps[x]);
                x.SetEmissiveColour(materialsToEmissiveColours[x]);
            });
    }
}
