using UnityEngine;

public class AbilityObjectPrefabLookup : Singleton<AbilityObjectPrefabLookup>
{
    [SerializeField]
    private FireballObject _fireballObjectPrefab = null;

    [SerializeField]
    private DaggerObject _daggerObjectPrefab = null;

    [SerializeField]
    private GameObject _slashObjectPrefab = null;

    public FireballObject FireballObjectPrefab { get => _fireballObjectPrefab; }
    public DaggerObject DaggerObjectPrefab { get => _daggerObjectPrefab; }
    public GameObject SlashObjectPrefab { get => _slashObjectPrefab; }
}
