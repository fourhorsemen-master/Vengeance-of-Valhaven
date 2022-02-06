using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmissiveManager
{
    private readonly Dictionary<Material, Texture> materialsToEmissiveMaps;
    private readonly Dictionary<Material, Color> materialsToEmissiveColours;
    private readonly Actor actor;

    private const float FlashIntensity = 0.3f;
    private const float FlashDuration = 0.1f;

    private Coroutine cancelFlashCoroutine = null;

    public EmissiveManager(Actor actor)
    {
        this.actor = actor;

        Renderer[] skinnedMeshRenderers = actor.GetComponentsInChildren<SkinnedMeshRenderer>();
        Renderer[] meshRenderers = actor.GetComponentsInChildren<MeshRenderer>();
        Renderer[] renderers = skinnedMeshRenderers.Concat(meshRenderers).ToArray();

        materialsToEmissiveMaps = renderers.ToDictionary(
            r => r.material,
            r => r.material.GetEmissiveMap()
        );
        materialsToEmissiveColours = renderers.ToDictionary(
            r => r.material,
            r => r.material.GetEmissiveColour()
        );
    }

    public void SetBaseEmissiveColour(List<MeshRenderer> renderers, Color colour)
    {
        renderers.ForEach(r => materialsToEmissiveColours[r.material] = colour);
        ResetEmissive();
    }

    public void Flash()
    {
        if (cancelFlashCoroutine != null) actor.StopCoroutine(cancelFlashCoroutine);

        SetFlashEmissive();

        cancelFlashCoroutine = actor.WaitAndAct(FlashDuration, ResetEmissive);
    }

    private void SetFlashEmissive()
    {
        foreach (Material material in materialsToEmissiveMaps.Keys)
        {
            material.SetEmissiveMap(Texture2D.whiteTexture);
            material.SetEmissiveColour(Color.white * FlashIntensity);
        }
    }

    private void ResetEmissive()
    {
        foreach (Material material in materialsToEmissiveMaps.Keys)
        {
            material.SetEmissiveMap(materialsToEmissiveMaps[material]);
            material.SetEmissiveColour(materialsToEmissiveColours[material]);
        }
    }
}
