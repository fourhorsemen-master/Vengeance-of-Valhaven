using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is only to be used for dev purposes and should not be active in a real build.
///
/// This manager can be enabled in scene so that scenes can be worked on individually without coming in through
/// the gameplay entry scene. This class overrides the methods of the real persistence manager and just returns
/// mock data that is enough for a scene to work when running in isolation.
/// </summary>
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

    public override void TransitionToNextRoom(int nextRoomId) {}

    public override void TransitionToDefeatRoom() {}

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
            CurrentRoomId = 0,
            RoomSaveDataLookup = new Dictionary<int, RoomSaveData>
            {
                {
                    0,
                    new RoomSaveData
                    {
                        RoomType = RoomType.Combat,
                        CombatRoomSaveData = new CombatRoomSaveData
                        {
                            EnemiesCleared = false,
                            SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>()
                        },
                        ModuleSeed = 0
                    }
                }
            }
        };
    }
}
