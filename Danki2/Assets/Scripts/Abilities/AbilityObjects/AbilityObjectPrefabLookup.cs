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
    private GameObject _slashObjectPrefab = null;

    [SerializeField]
    private WhirlwindObject _whirlwindObjectPrefab = null;

    [SerializeField]
    private RollObject _rollObjectPrefab = null;

    public FireballObject FireballObjectPrefab { get => _fireballObjectPrefab; }
    public DaggerObject DaggerObjectPrefab { get => _daggerObjectPrefab; }
    public BiteObject BiteObjectPrefab { get => _biteObjectPrefab; }
    public PounceObject PounceObjectPrefab { get => _pounceObjectPrefab; }
    public LungeObject LungeObjectPrefab { get => _lungeObjectPrefab; }
    public SmashObject SmashObjectPrefab { get => _smashObjectPrefab; }
    public WhirlwindObject WhirlwindObjectPrefab { get => _whirlwindObjectPrefab; }
    public GameObject SlashObjectPrefab { get => _slashObjectPrefab; }
    public RollObject RollObjectPrefab { get => _rollObjectPrefab; }
}
