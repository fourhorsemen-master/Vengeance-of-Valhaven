using UnityEngine;

public class WorldSpaceEffectList : EffectList
{
    [SerializeField]
    private Actor actor = null;

    [SerializeField]
    private EffectListItem worldSpaceEffectListItemPrefab = null;

    protected override Actor Actor => actor;

    protected override EffectListItem EffectListItemPrefab => worldSpaceEffectListItemPrefab;
}
