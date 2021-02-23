using UnityEngine;

public class AbilityObjectPrefabLookup : Singleton<AbilityObjectPrefabLookup>
{
    [SerializeField]
    private BarbedDaggerObject _barbedDaggerObjectPrefab = null;

    [SerializeField]
    private BiteObject _biteObjectPrefab = null;

    [SerializeField]
    private PounceObject _pounceObjectPrefab = null;

    [SerializeField]
    private LungeObject _lungeObjectPrefab = null;

    [SerializeField]
    private SmashObject _smashObjectPrefab = null;

    [SerializeField]
    private SlashObject _slashObjectPrefab = null;

    [SerializeField]
    private WhirlwindObject _whirlwindObjectPrefab = null;

    [SerializeField]
    private BashObject _bashObjectPrefab = null;

    [SerializeField]
    private SweepingStrikeObject _sweepingStrikeObjectPrefab = null;

    [SerializeField]
    private GrapplingHookObject _grapplingHookObjectPrefab = null;

    [SerializeField]
    private BackstabObject _backstabObjectPrefab = null;

    [SerializeField]
    private PiercingRushObject _piercingRushObjectPrefab = null;

    [SerializeField]
    private ParryObject _parryObjectPrefab = null;

    [SerializeField]
    private RendObject _rendObjectPrefab = null;

    [SerializeField]
    private DisengageObject _disengageObjectPrefab = null;

    [SerializeField]
    private FanOfKnivesObject _fanOfKnivesObjectPrefab = null;

    [SerializeField]
    private HamstringObject _hamstringObjectPrefab = null;

    [SerializeField]
    private SwipeObject _swipeObjectPrefab = null;

    [SerializeField]
    private MaulObject _maulObjectPrefab = null;

    [SerializeField]
    private BearChargeObject _bearChargeObjectPrefab = null;

    [SerializeField]
    private ExecuteObject _executeObjectPrefab = null;

    public BarbedDaggerObject BarbedDaggerObjectPrefab => _barbedDaggerObjectPrefab;
    public BiteObject BiteObjectPrefab => _biteObjectPrefab;
    public PounceObject PounceObjectPrefab => _pounceObjectPrefab;
    public LungeObject LungeObjectPrefab => _lungeObjectPrefab;
    public SmashObject SmashObjectPrefab => _smashObjectPrefab;
    public WhirlwindObject WhirlwindObjectPrefab => _whirlwindObjectPrefab;
    public SlashObject SlashObjectPrefab => _slashObjectPrefab;
    public BashObject BashObjectPrefab => _bashObjectPrefab;
    public SweepingStrikeObject SweepingStrikeObjectPrefab => _sweepingStrikeObjectPrefab;
    public GrapplingHookObject GrapplingHookObjectPrefab => _grapplingHookObjectPrefab;
    public BackstabObject BackstabObjectPrefab => _backstabObjectPrefab;
    public PiercingRushObject PiercingRushObjectPrefab => _piercingRushObjectPrefab;
    public ParryObject ParryObjectPrefab => _parryObjectPrefab;
    public RendObject RendObjectPrefab => _rendObjectPrefab;
    public DisengageObject DisengageObjectPrefab => _disengageObjectPrefab;
    public FanOfKnivesObject FanOfKnivesObjectPrefab => _fanOfKnivesObjectPrefab;
    public HamstringObject HamstringObjectPrefab => _hamstringObjectPrefab;
    public SwipeObject SwipeObjectPrefab => _swipeObjectPrefab;
    public MaulObject MaulObjectPrefab => _maulObjectPrefab;
    public BearChargeObject BearChargeObjectPrefab => _bearChargeObjectPrefab;
    public ExecuteObject ExecuteObjectPrefab => _executeObjectPrefab;
}
