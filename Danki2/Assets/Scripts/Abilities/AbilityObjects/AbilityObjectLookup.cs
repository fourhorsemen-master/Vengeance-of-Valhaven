using UnityEngine;

public class AbilityObjectLookup : Singleton<AbilityObjectLookup>
{
    [SerializeField]
    private FireballObject _fireballObjectPrefab;

    public FireballObject FireballObjectPrefab { get => _fireballObjectPrefab; }
}
