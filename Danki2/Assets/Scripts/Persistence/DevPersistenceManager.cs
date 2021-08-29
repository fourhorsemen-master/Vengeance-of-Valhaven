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
    [SerializeField] public string leftAbilityName = "";
    [SerializeField] public string rightAbilityName = "";
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
    [SerializeField] public List<string> abilityChoiceNames = new List<string>();
    [SerializeField] public bool hasHealed = false;
    [SerializeField] public TextAsset abilityNameStore = null;

    public override SaveData SaveData => GenerateNewSaveData();

    protected override bool DestroyOnLoad => true;

    protected override void Start() {}

    public override void Save() {}

    public override void TransitionToNextRoom(RoomNode nextRoomNode) {}

    public override void TransitionToDefeatRoom() {}

    private SaveData GenerateNewSaveData()
    {
        Dictionary<SerializableGuid, int> ownedAbilities = new Dictionary<SerializableGuid, int>();
        AbilityLookup2.Instance.ForEachAbilityId(abilityId => ownedAbilities[abilityId] = ownedAbilityCount);

        if (!AbilityLookup2.Instance.TryGetAbilityId(leftAbilityName, out SerializableGuid leftAbilityId))
        {
            Debug.LogError($"Invalid left starting ability name: {leftAbilityName}.");
        }

        if (!AbilityLookup2.Instance.TryGetAbilityId(rightAbilityName, out SerializableGuid rightAbilityId))
        {
            Debug.LogError($"Invalid right starting ability name: {rightAbilityName}.");
        }

        List<SerializableGuid> abilityChoices = new List<SerializableGuid>();
        abilityChoiceNames.ForEach(abilityName =>
        {
            if (!AbilityLookup2.Instance.TryGetAbilityId(abilityName, out SerializableGuid abilityId))
            {
                Debug.LogError($"Invalid ability name in ability choices: {abilityName}.");
                return;
            }

            abilityChoices.Add(abilityId);
        });
        
        return new SaveData
        {
            PlayerHealth = playerHealth,
            SerializableAbilityTree = AbilityTreeFactory.CreateTree(
                ownedAbilities,
                AbilityTreeFactory.CreateNode(leftAbilityId),
                AbilityTreeFactory.CreateNode(rightAbilityId)
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
