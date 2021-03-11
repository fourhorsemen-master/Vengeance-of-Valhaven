using UnityEngine;

public class ScreenSpaceEffectList : EffectList
{
    [SerializeField]
    private EffectListItem screenSpaceEffectListItemPrefab = null;

    protected override Actor Actor => ActorCache.Instance.Player;

    protected override EffectListItem EffectListItemPrefab => screenSpaceEffectListItemPrefab;
}
