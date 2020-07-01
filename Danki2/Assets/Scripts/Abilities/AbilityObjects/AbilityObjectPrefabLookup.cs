using UnityEngine;

public class AbilityObjectPrefabLookup : Singleton<AbilityObjectPrefabLookup>
{
    [SerializeField]
    private FireballObject _fireballObjectPrefab = null;

    [SerializeField]
    private DaggerObject _daggerObjectPrefab = null;

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
    private DashObject _dashObjectPrefab = null;

    [SerializeField]
    private LeapObject _leapObjectPrefab = null;

    [SerializeField]
    private LeechingStrikeObject _leechingStrikeObjectPrefab = null;

    [SerializeField]
    private MeditateObject _meditateObjectPrefab = null;

    [SerializeField]
    private BashObject _bashObjectPrefab = null;

    [SerializeField]
    private SweepingStrikeObject _sweepingStrikeObjectPrefab = null;

    [SerializeField]
    private HookObject _hookObjectPrefab = null;

    [SerializeField]
    private BackstabObject _backstabObjectPrefab = null;

    [SerializeField]
    private PiercingRushObject _piercingRushObjectPrefab = null;

    public FireballObject FireballObjectPrefab => _fireballObjectPrefab;
    public DaggerObject DaggerObjectPrefab => _daggerObjectPrefab;
    public BiteObject BiteObjectPrefab => _biteObjectPrefab;
    public PounceObject PounceObjectPrefab => _pounceObjectPrefab;
    public LungeObject LungeObjectPrefab => _lungeObjectPrefab;
    public SmashObject SmashObjectPrefab => _smashObjectPrefab;
    public WhirlwindObject WhirlwindObjectPrefab => _whirlwindObjectPrefab;
    public SlashObject SlashObjectPrefab => _slashObjectPrefab;
    public DashObject DashObjectPrefab => _dashObjectPrefab;
    public LeapObject LeapObjectPrefab => _leapObjectPrefab;
    public LeechingStrikeObject LeechingStrikeObjectPrefab => _leechingStrikeObjectPrefab;
    public MeditateObject MeditateObjectPrefab => _meditateObjectPrefab;
    public BashObject BashObjectPrefab => _bashObjectPrefab;
    public SweepingStrikeObject SweepingStrikeObjectPrefab => _sweepingStrikeObjectPrefab;
    public HookObject HookObjectPrefab => _hookObjectPrefab;
    public BackstabObject BackstabObjectPrefab => _backstabObjectPrefab;
    public PiercingRushObject PiercingRushObjectPrefab => _piercingRushObjectPrefab;
}
