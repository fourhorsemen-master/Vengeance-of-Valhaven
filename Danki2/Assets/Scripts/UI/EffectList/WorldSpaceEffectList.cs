using UnityEngine;

public class WorldSpaceEffectList : EffectList
{
    [SerializeField]
    private Diegetic diegetic = null;

    [SerializeField]
    private EffectListItem worldSpaceEffectListItemPrefab = null;

    protected override Actor Actor => diegetic.Actor;

    protected override EffectListItem EffectListItemPrefab => worldSpaceEffectListItemPrefab;
}
