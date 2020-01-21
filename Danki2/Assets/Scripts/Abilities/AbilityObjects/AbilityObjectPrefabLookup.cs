using UnityEngine;

public class AbilityObjectPrefabLookup : Singleton<AbilityObjectPrefabLookup>
{
    [SerializeField]
    private FireballObject _fireballObjectPrefab = null;

    [SerializeField]
    private DaggerObject _daggerObjectPrefab = null;

    public FireballObject FireballObjectPrefab { get => _fireballObjectPrefab; }
    public DaggerObject DaggerObjectPrefab { get => _daggerObjectPrefab; }
}
