using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularPFXComponent : MonoBehaviour
{
    [SerializeField]
    private MPFXSettings _settings;

    [SerializeField]
    private MPFXBehaviour[] _behaviours;

    private GameObject _spawnedGraphic;
    private bool isPlaying = false;
    private int stencilsCompleted = 0;

    public void Start()
    {
        BeginPFX();
    }

    public void BeginPFX()
    {
        if (!_settings.effectObject) return;

        _spawnedGraphic = Instantiate(_settings.effectObject, transform);
        SetEffectColour();

        if (isPlaying) return;

        isPlaying = true;

        foreach (MPFXBehaviour stencil in _behaviours)
        {
            stencil.SetUp(_spawnedGraphic);
        }

        StartCoroutine("UpdatePFX");
    }

    public void Update()
    {
        if (isPlaying) UpdatePFX();
    }

    private void UpdatePFX()
    {
        foreach(MPFXBehaviour stencil in _behaviours)
        {
            if (stencil.UpdatePFX()) ++stencilsCompleted;
        }

        if (stencilsCompleted == _behaviours.Length)
        {
            EndPFX();
        }
    }

    private void EndPFX()
    {
        stencilsCompleted = 0;

        foreach (MPFXBehaviour stencil in _behaviours)
        {
            stencil.End();
        }

        isPlaying = false;
        Destroy(_spawnedGraphic);
        StopCoroutine("UpdatePFX");
    }

    private void SetEffectColour()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mesh in meshes)
        {
            //These magic strings are awful, but are needed to workaround a bug in our verion of Unity
            //These allow for the emissive colour to be altererd at runtime, which is broken when trying
            //to alter these through the built in shader variables.
            mesh.material.SetColor("Color_33632292", _settings.effectColor);
            mesh.material.SetColor("Color_A9688267", _settings.effectEmissive);
        }
    }
}
 