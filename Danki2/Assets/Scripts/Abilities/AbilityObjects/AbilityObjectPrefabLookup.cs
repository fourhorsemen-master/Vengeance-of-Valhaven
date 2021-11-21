using UnityEngine;

public class AbilityObjectPrefabLookup : Singleton<AbilityObjectPrefabLookup>
{
    [SerializeField]
    private BiteObject _biteObjectPrefab = null;

    [SerializeField]
    private SmashObject _smashObjectPrefab = null;

    [SerializeField]
    private SlashObject _slashObjectPrefab = null;

    [SerializeField]
    private SwipeObject _swipeObjectPrefab = null;

    [SerializeField]
    private MaulObject _maulObjectPrefab = null;

    [SerializeField]
    private CleaveObject _cleaveObjectPrefab = null;

    [SerializeField]
    private PoisonStabVisual _poisonStabVisualPrefab = null;

    [SerializeField]
    private SpineObject _spineObjectPrefab = null;

    [SerializeField]
    private GuidedOrbObject _guidedOrbObjectPrefab = null;

    [SerializeField]
    private WraithSwipeObject _wraithSwipeObjectPrefab = null;

    [SerializeField]
    private LightningChainVisual _lightningChainVisualPrefab = null;

    [SerializeField]
    private LightningImpactVisual _lightningImpactVisualPrefab = null;

    public BiteObject BiteObjectPrefab => _biteObjectPrefab;
    public SmashObject SmashObjectPrefab => _smashObjectPrefab;
    public SlashObject SlashObjectPrefab => _slashObjectPrefab;
    public SwipeObject SwipeObjectPrefab => _swipeObjectPrefab;
    public MaulObject MaulObjectPrefab => _maulObjectPrefab;
    public CleaveObject CleaveObjectPrefab => _cleaveObjectPrefab;
    public PoisonStabVisual PoisonStabVisualPrefab => _poisonStabVisualPrefab;
    public SpineObject SpineObjectPrefab => _spineObjectPrefab;
    public GuidedOrbObject GuidedOrbObjectPrefab => _guidedOrbObjectPrefab;
    public WraithSwipeObject WraithSwipeObjectPrefab => _wraithSwipeObjectPrefab;
    public LightningChainVisual LightningChainVisualPrefab => _lightningChainVisualPrefab;
    public LightningImpactVisual LightningImpactVisualPrefab => _lightningImpactVisualPrefab;
}
