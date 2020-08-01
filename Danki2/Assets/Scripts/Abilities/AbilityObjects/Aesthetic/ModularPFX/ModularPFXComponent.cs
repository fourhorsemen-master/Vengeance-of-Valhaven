using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularPFXComponent : MonoBehaviour
{

    [SerializeField, HideInInspector]
    Material _mpfxMaterial = null;

    [SerializeField]
    private MPFXSettings _settings;

    [SerializeField]
    private MPFXBehaviour[] _behaviours;

    private GameObject _spawnedGraphic;
    private bool _isPlaying = false;

    public void Start()
    {
        BeginPFX();
    }

    public void BeginPFX()
    {
        if (!_settings.effectObject) return;

        _spawnedGraphic = Instantiate(_settings.effectObject, transform);
        SetEffectColour();

        if (_isPlaying) return;

        _isPlaying = true;

        foreach (MPFXBehaviour stencil in _behaviours)
        {
            stencil.SetUp(_spawnedGraphic);
        }

        StartCoroutine("UpdatePFX");
    }

    public void Update()
    {
        if (_isPlaying) UpdatePFX();
    }

    private void UpdatePFX()
    {
        int behavioursComplete = 0;

        foreach(MPFXBehaviour stencil in _behaviours)
        {
            if (stencil.UpdatePFX())
            {
                ++behavioursComplete;
            }
        }

        if (behavioursComplete == _behaviours.Length)
        {
            EndPFX();
        }
    }

    private void EndPFX()
    {
        foreach (MPFXBehaviour stencil in _behaviours)
        {
            stencil.End();
        }

        _isPlaying = false;
        Destroy(_spawnedGraphic);
        StopCoroutine("UpdatePFX");
    }

    private void SetEffectColour()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mesh in meshes)
        {
            mesh.material = _mpfxMaterial;

            //These magic strings are awful, but are needed to workaround a bug in our verion of Unity
            //These allow for the emissive colour to be altererd at runtime, which is broken when trying
            //to alter these through the built in shader variables.
            mesh.material.SetColor("Color_33632292", _settings.effectColor);
            mesh.material.SetColor("Color_A9688267", _settings.effectEmissive);
        }
    }
}
 