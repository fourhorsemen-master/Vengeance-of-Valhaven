﻿using System.Dynamic;
using UnityEngine;

public class ModularPFXComponent : MonoBehaviour
{
    public const string ColourKeyString = "Color_33632292";
    const string EmissiveKeyString = "Color_A9688267";
    const string AlbedoKeyString = "Texture2D_1FF4AC07";

    [SerializeField]
    Material mpfxMaterial = null;

    [SerializeField]
    private MPFXSettings settings;

    [SerializeField]
    private MPFXBehaviour[] behaviours = null;

    private GameObject spawnedGraphic;

    private bool isActive = true;

    public void Start()
    {
        if (!settings.effectObject) return;

        spawnedGraphic = Instantiate(settings.effectObject, transform);
        SetEffectColour();

        foreach (MPFXBehaviour behaviour in behaviours)
        {
            behaviour.SetUp(spawnedGraphic, this);
        }
    }

    public void Update()
    {
        if (!isActive) return;

        int behavioursComplete = 0;

        foreach (MPFXBehaviour behaviour in behaviours)
        {
            if (behaviour.UpdatePFX())
            {
                ++behavioursComplete;
            }
        }

        if (behavioursComplete == behaviours.Length)
        {
            EndPFX();
        }
    }

    private void EndPFX()
    {
        foreach (MPFXBehaviour behaviour in behaviours)
        {
            behaviour.End();
        }

        Destroy(spawnedGraphic);
        isActive = false;
    }

    private void SetEffectColour()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mesh in meshes)
        {
            mesh.material = mpfxMaterial;

            //These magic strings are awful, but are needed to workaround a bug in our version of Unity
            //These allow for the emissive colour to be altered at runtime, which is broken when trying
            //to alter these through the built in shader variables.
            mesh.material.SetColor(ColourKeyString, settings.effectColor);
            mesh.material.SetColor(EmissiveKeyString, settings.effectEmissive);
            mesh.material.SetTexture(AlbedoKeyString, settings.effectAlbedo);
        }
    }
}
 