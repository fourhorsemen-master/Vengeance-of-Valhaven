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
    private GameObject _slashObjectPrefab = null;

    public FireballObject FireballObjectPrefab { get => _fireballObjectPrefab; }
    public DaggerObject DaggerObjectPrefab { get => _daggerObjectPrefab; }
    public BiteObject BiteObjectPrefab { get => _biteObjectPrefab; }
}
