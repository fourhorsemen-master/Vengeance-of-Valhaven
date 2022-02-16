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
    [SerializeField] public AbilityType leftAbilitytype = default;
    [SerializeField] public List<Empowerment> leftAbilityEmpowerments = null;
    [SerializeField] public AbilityType rightAbilitytype = default;
    [SerializeField] public List<Empowerment> rightAbilityEmpowerments = null;
    [SerializeField] public int playerHealth = 0;
    [SerializeField] public List<RuneSocket> runeSockets = new List<RuneSocket>();
    [SerializeField] public List<Rune> runeOrder = new List<Rune>();
    [SerializeField] public int currencyAmount = 0;
    [SerializeField] public Pole cameraOrientation = Pole.North;
    [SerializeField] public bool enemiesCleared = false;
    [SerializeField] public List<SpawnedEnemy> spawnedEnemies = new List<SpawnedEnemy>();
    [SerializeField] public List<int> activeTransitions = new List<int>();
    [SerializeField] public bool useRandomSeeds = true;
    [SerializeField] public int moduleSeed = 0;
    [SerializeField] public int transitionModuleSeed = 0;
    [SerializeField] public int playerSpawnerId = 0;
    [SerializeField] public int depth = 0;
    [SerializeField] public Zone zone = Zone.Zone1;
    [SerializeField] public int depthInZone = 0;
    [SerializeField] public RoomType roomType = RoomType.Combat;
    [SerializeField] public bool hasHealed = false;
    [SerializeField] public AbilityData abilityData = null;

    private SaveData saveData = null;

    public override SaveData SaveData => GetSaveData();

    protected override bool DestroyOnLoad => true;

    protected override void Start() {}

    public override void Save() {}

    public override void TransitionToNextRoom(RoomNode nextRoomNode) {}

    public override void TransitionToDefeatRoom() {}

    private SaveData GetSaveData()
    {
        if (saveData == null) saveData = GenerateNewSaveData();

        return saveData;
    }

    private SaveData GenerateNewSaveData()
    {
        List<Ability> abilityInventory = new List<Ability>();

        Ability leftStartingAbility = new Ability("LeftAbility", leftAbilitytype, 6, Rarity.Common, leftAbilityEmpowerments);

        Ability rightStartingAbility = new Ability("rightAbility", rightAbilitytype, 6, Rarity.Common, rightAbilityEmpowerments);

        List<Ability> abilityChoices = new List<Ability>();

        AbilityGenerator generator = new AbilityGenerator(zone, 0.5f);

        Utils.Repeat(3, () =>
        {
            Ability ability = generator.Generate();
            abilityChoices.Add(ability);
        });

        return new SaveData
        {
            PlayerHealth = playerHealth,
            SerializableAbilityTree = AbilityTreeFactory.CreateTree(
                abilityInventory,
                AbilityTreeFactory.CreateNode(leftStartingAbility),
                AbilityTreeFactory.CreateNode(rightStartingAbility)
            ).Serialize(),
            RuneSockets = runeSockets,
            RuneOrder = runeOrder,
            CurrencyAmount = currencyAmount,
            CurrentRoomNode = new RoomNode
            {
                Depth = depth,
                Zone = zone,
                DepthInZone = depthInZone,
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
