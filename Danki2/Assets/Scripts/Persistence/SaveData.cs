using System.Collections.Generic;
using System.Linq;

public class SaveData
{
    public int Version { get; set; }

    public int PlayerHealth { get; set; }
    public AbilityTree AbilityTree { get; set; }

    public int CurrentSceneId { get; set; }
    public Dictionary<int, SceneSaveData> SceneSaveDataLookup { get; set; }
    public Dictionary<int, List<int>> SceneTransitions { get; set; }

    public SerializableSaveData Serialize()
    {
        return new SerializableSaveData
        {
            Version = Version,
            PlayerHealth = PlayerHealth,
            SerializableAbilityTree = new SerializableAbilityTree(AbilityTree),
            CurrentSceneId = CurrentSceneId,
            SerializableSceneSaveDataList = SceneSaveDataLookup.Values
                .Select(sceneData => sceneData.Serialize())
                .ToList(),
            SerializableSceneTransitions = SceneTransitions.Keys
                .Select(fromId => new SerializableSceneTransition
                {
                    FromId = fromId,
                    ToIds = SceneTransitions[fromId]
                })
                .ToList()
        };
    }
}

public class SceneSaveData
{
    public int Id { get; set; }
    public Scene Scene { get; set; }
    public SceneType SceneType { get; set; }
    public CombatSceneSaveData CombatSceneSaveData { get; set; }

    public SerializableSceneSaveData Serialize()
    {
        return new SerializableSceneSaveData
        {
            Id = Id,
            Scene = Scene,
            SceneType = SceneType,
            SerializableCombatSceneSaveData = CombatSceneSaveData?.Serialize()
        };
    }
}

public class CombatSceneSaveData
{
    public bool EnemiesCleared { get; set; }
    public Dictionary<int, ActorType> SpawnerIdToSpawnedActor { get; set; }

    public SerializableCombatSceneSaveData Serialize()
    {
        return new SerializableCombatSceneSaveData
        {
            EnemiesCleared = EnemiesCleared,
            SerializableSpawnerSaveDataList = SpawnerIdToSpawnedActor.Keys
                .Select(spawnerId => new SerializableSpawnerSaveData
                {
                    Id = spawnerId,
                    ActorType = SpawnerIdToSpawnedActor[spawnerId]
                })
                .ToList()
        };
    }
}

public enum SceneType
{
    Combat,
    Shop,
    Victory
}

