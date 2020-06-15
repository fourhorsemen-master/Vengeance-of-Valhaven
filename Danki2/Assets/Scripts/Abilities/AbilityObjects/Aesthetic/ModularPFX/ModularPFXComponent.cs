using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularPFXComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _graphic;

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
        if (!_graphic) return;

        _spawnedGraphic = Instantiate(_graphic, transform);

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
}
