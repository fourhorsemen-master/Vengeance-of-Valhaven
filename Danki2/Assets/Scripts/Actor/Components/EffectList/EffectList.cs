using UnityEngine;

public enum EffectRenderMode
{
    WorldSpace,
    ScreenSpace
}

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
        if (effectRenderMode.Equals(EffectRenderMode.WorldSpace))
        {
            Instantiate(worldSpaceEffectListItemPrefab, transform);
            return;
        }
        
        if (effectRenderMode.Equals(EffectRenderMode.ScreenSpace))
        {
            Instantiate(screenSpaceEffectListItemPrefab, transform);
        }
    }
}
