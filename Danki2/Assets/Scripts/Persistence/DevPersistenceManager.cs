using System.Collections.Generic;
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

    public override void TransitionToNextScene() {}

    private SaveData GenerateNewSaveData()
    {
        return new SaveData
        {
            PlayerHealth = playerHealth,
            AbilityTree = AbilityTreeFactory.CreateTree(
            new EnumDictionary<AbilityReference, int>(ownedAbilityCount),
            AbilityTreeFactory.CreateNode(leftAbility),
            AbilityTreeFactory.CreateNode(rightAbility)
        ),
            CurrentSceneId = 0,
            SceneSaveDataLookup = new Dictionary<int, SceneSaveData>
            {
                [0] = new SceneSaveData
                {
                    SceneType = SceneType.Combat,
                    CombatSceneSaveData = new CombatSceneSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Wolf
                        }
                    }
                }
            }
        };
    }
}
