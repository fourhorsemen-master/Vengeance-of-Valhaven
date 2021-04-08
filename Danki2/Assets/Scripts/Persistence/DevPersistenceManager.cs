using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This class is only to be used for dev purposes and should not be active in a real build.
///
/// This manager can be enabled in scene so that scenes can be worked on individually without coming in through
/// the gameplay entry scene. This class overrides the methods of the real persistence manager and just returns
/// mock data that is enough for a scene to work when running in isolation.
/// </summary>
public class DevPersistenceManager : PersistenceManager
{
    [SerializeField] public int ownedAbilityCount = 0;
    [SerializeField] public AbilityReference leftAbility = AbilityReference.Slash;
    [SerializeField] public AbilityReference rightAbility = AbilityReference.Slash;
    [SerializeField] public int playerHealth = 0;
    [SerializeField] public Pole cameraOrientation = Pole.North;
    [SerializeField] public List<SpawnedEnemy> spawnedEnemies = new List<SpawnedEnemy>();
    [SerializeField] public bool useRandomSeeds = true;
    [SerializeField] public int moduleSeed = 0;
    [SerializeField] public int transitionModuleSeed = 0;
    [SerializeField] public int playerSpawnerId = 0;
    [SerializeField] public RoomType roomType = RoomType.Combat;
    [SerializeField] public AbilityReference abilityChoice1 = AbilityReference.Slash;
    [SerializeField] public AbilityReference abilityChoice2 = AbilityReference.Slash;
    [SerializeField] public AbilityReference abilityChoice3 = AbilityReference.Slash;

    public override SaveData SaveData => GenerateNewSaveData();

    protected override bool DestroyOnLoad => true;

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
            SerializableAbilityTree = AbilityTreeFactory.CreateTree(
                new EnumDictionary<AbilityReference, int>(ownedAbilityCount),
                AbilityTreeFactory.CreateNode(leftAbility),
                AbilityTreeFactory.CreateNode(rightAbility)
            ).Serialize(),
            CurrentRoomId = 0,
            RoomSaveDataLookup = new Dictionary<int, RoomSaveData>
            {
                [0] = new RoomSaveData
                {
                    RoomType = roomType,
                    CombatRoomSaveData = new CombatRoomSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = spawnedEnemies.ToDictionary(
                            spawnedEnemy => spawnedEnemy.SpawnerId,
                            spawnedEnemy => spawnedEnemy.ActorType
                        )
                    },
                    AbilityRoomSaveData = new AbilityRoomSaveData
                    {
                        AbilityChoices = new List<AbilityReference>
                        {
                            abilityChoice1,
                            abilityChoice2,
                            abilityChoice3
                        },
                        AbilitiesViewed = false,
                        AbilitySelected = false
                    },
                    RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                    {
                        [3] = 1
                    },
                    ModuleSeed = useRandomSeeds ? RandomUtils.Seed() : moduleSeed,
                    TransitionModuleSeed = useRandomSeeds ? RandomUtils.Seed() : transitionModuleSeed,
                    CameraOrientation = cameraOrientation,
                    PlayerSpawnerId = playerSpawnerId
                }
            }
        };
    }

    [Serializable]
    public class SpawnedEnemy
    {
        [SerializeField] private int spawnerId;
        [SerializeField] private ActorType actorType;

        public int SpawnerId { get => spawnerId; set => spawnerId = value; }
        public ActorType ActorType { get => actorType; set => actorType = value; }
    }
}
