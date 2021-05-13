using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] public int ownedAbilityCount = 0;
    [SerializeField] public AbilityReference leftAbility = AbilityReference.Slash;
    [SerializeField] public AbilityReference rightAbility = AbilityReference.Slash;
    [SerializeField] public int playerHealth = 0;
    [SerializeField] public List<RuneSocket> runeSockets = new List<RuneSocket>();
    [SerializeField] public List<Rune> runeOrder = new List<Rune>();
    [SerializeField] public Pole cameraOrientation = Pole.North;
    [SerializeField] public bool enemiesCleared = false;
    [SerializeField] public List<SpawnedEnemy> spawnedEnemies = new List<SpawnedEnemy>();
    [SerializeField] public List<int> activeTransitions = new List<int>();
    [SerializeField] public bool useRandomSeeds = true;
    [SerializeField] public int moduleSeed = 0;
    [SerializeField] public int transitionModuleSeed = 0;
    [SerializeField] public int playerSpawnerId = 0;
    [SerializeField] public int depth = 0;
    [SerializeField] public RoomType roomType = RoomType.Combat;
    [SerializeField] public List<AbilityReference> abilityChoices = new List<AbilityReference>();
    [SerializeField] public bool hasHealed = false;

    public override SaveData SaveData => GenerateNewSaveData();

    protected override bool DestroyOnLoad => true;

    protected override void Start() {}

    public override void Save() {}

    public override void TransitionToNextRoom(RoomNode nextRoomNode) {}

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
            RuneSockets = runeSockets,
            RuneOrder = runeOrder,
            CurrentRoomNode = new RoomNode
            {
                Depth = depth,
                RoomType = roomType,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = enemiesCleared,
                    SpawnerIdToSpawnedActor = spawnedEnemies.ToDictionary(
                        spawnedEnemy => spawnedEnemy.SpawnerId,
                        spawnedEnemy => spawnedEnemy.ActorType
                    )
                },
                AbilityRoomSaveData = new AbilityRoomSaveData
                {
                    AbilityChoices = abilityChoices,
                    AbilitiesViewed = false,
                    AbilitySelected = false
                },
                HealingRoomSaveData = new HealingRoomSaveData
                {
                    HasHealed = hasHealed
                },
                RuneRoomSaveData = new RuneRoomSaveData
                {
                    RunesViewed = false,
                    RuneSelected = false
                },
                ExitIdToChildLookup = activeTransitions.ToDictionary(
                    id => id,
                    id => (RoomNode) null
                ),
                ExitIdToIndicatesNextRoomType = activeTransitions.ToDictionary(
                    id => id,
                    id => false
                ),
                ExitIdToFurtherIndicatedRoomTypes = activeTransitions.ToDictionary(
                    id => id,
                    id => new List<RoomType>()
                ),
                ModuleSeed = useRandomSeeds ? RandomUtils.Seed() : moduleSeed,
                TransitionModuleSeed = useRandomSeeds ? RandomUtils.Seed() : transitionModuleSeed,
                CameraOrientation = cameraOrientation,
                PlayerSpawnerId = playerSpawnerId
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
