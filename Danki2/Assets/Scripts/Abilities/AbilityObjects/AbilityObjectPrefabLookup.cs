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

    public FireballObject FireballObjectPrefab { get => _fireballObjectPrefab; }
    public DaggerObject DaggerObjectPrefab { get => _daggerObjectPrefab; }
    public BiteObject BiteObjectPrefab { get => _biteObjectPrefab; }
    public PounceObject PounceObjectPrefab { get => _pounceObjectPrefab; }
    public LungeObject LungeObjectPrefab { get => _lungeObjectPrefab; }
    public SmashObject SmashObjectPrefab { get => _smashObjectPrefab; }
    public WhirlwindObject WhirlwindObjectPrefab { get => _whirlwindObjectPrefab; }
    public SlashObject SlashObjectPrefab { get => _slashObjectPrefab; }
    public RollObject DashObjectPrefab { get => _dashObjectPrefab; }
    public LeapObject LeapObjectPrefab { get => _leapObjectPrefab; }
}
