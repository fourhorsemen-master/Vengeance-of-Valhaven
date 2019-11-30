using UnityEngine;

public class AbilityObjectLookup : Singleton<AbilityObjectLookup>
{
    [SerializeField]
    private FireballObject _fireballPrefab;

    public FireballObject FireballPrefab { get => _fireballPrefab; }
}
