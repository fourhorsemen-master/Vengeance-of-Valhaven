using UnityEngine;

public class DevPersistenceManager : PersistenceManager
{
    [SerializeField] private int ownedAbilityCount = 0;
    [SerializeField] private AbilityReference leftAbility = AbilityReference.Slash;
    [SerializeField] private AbilityReference rightAbility = AbilityReference.Slash;
    [SerializeField] private int playerHealth = 0;
    
    public override SaveData SaveData => GenerateNewSaveData();

    protected override void Awake()
    {
        base.Awake();
        Debug.LogWarning("Dev persistence manager is active, this change should not be committed.");
    }

    protected override void Start() {}

    public override void Save() {}

    private SaveData GenerateNewSaveData()
    {
        AbilityTree abilityTree = AbilityTreeFactory.CreateTree(
            new EnumDictionary<AbilityReference, int>(ownedAbilityCount),
            AbilityTreeFactory.CreateNode(leftAbility),
            AbilityTreeFactory.CreateNode(rightAbility)
        );

        return new SaveData(default, default, playerHealth, new SerializableAbilityTree(abilityTree));
    }
}
