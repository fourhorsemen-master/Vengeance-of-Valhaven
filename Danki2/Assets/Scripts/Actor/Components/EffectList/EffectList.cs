using System;
using UnityEngine;

public class EffectList : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private EffectListItem worldSpaceEffectListItemPrefab = null;
    
    [SerializeField, HideInInspector]
    private EffectListItem screenSpaceEffectListItemPrefab = null;

    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private EffectRenderMode effectRenderMode = EffectRenderMode.WorldSpace;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddImage();
        }
    }

    private void AddImage()
    {
        switch (effectRenderMode)
        {
            case EffectRenderMode.WorldSpace:
                Instantiate(worldSpaceEffectListItemPrefab, transform);
                break;
            case EffectRenderMode.ScreenSpace:
                Instantiate(screenSpaceEffectListItemPrefab, transform);
                break;
        }
    }
}
