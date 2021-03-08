using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableSaveData
{
    [SerializeField] private int version;
    [SerializeField] private int playerHealth;
    [SerializeField] private SerializableAbilityTree serializableAbilityTree;
    [SerializeField] private int currentSceneId;
    [SerializeField] private List<SerializableSceneSaveData> serializableSceneSaveDataList;
    [SerializeField] private List<SerializableSceneTransition> serializableSceneTransitions;
    
    public int Version { get => version; set => version = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public SerializableAbilityTree SerializableAbilityTree { get => serializableAbilityTree; set => serializableAbilityTree = value; }
    public int CurrentSceneId { get => currentSceneId; set => currentSceneId = value; }
    public List<SerializableSceneSaveData> SerializableSceneSaveDataList { get => serializableSceneSaveDataList; set => serializableSceneSaveDataList = value; }
    public List<SerializableSceneTransition> SerializableSceneTransitions { get => serializableSceneTransitions; set => serializableSceneTransitions = value; }

    public SaveData Deserialize()
    {
        return new SaveData
        {
            Version = Version,
            PlayerHealth = PlayerHealth,
            AbilityTree = SerializableAbilityTree.Deserialize(),
            CurrentSceneId = CurrentSceneId,
            SceneSaveDataLookup = SerializableSceneSaveDataList.ToDictionary(
                d => d.Id,
                d => d.Deserialize()
            ),
            SceneTransitions = SerializableSceneTransitions.ToDictionary(
                t => t.FromId,
                t => t.ToIds
            )
        };
    }
}

[Serializable]
public class SerializableSceneSaveData
{
    [SerializeField] private int id;
    [SerializeField] private Scene scene;
    [SerializeField] private SceneType sceneType;
    [SerializeField] private SerializableCombatSceneSaveData serializableCombatSceneSaveData;

    public int Id {get => id; set => id = value; }
    public SceneType SceneType {get => sceneType; set => sceneType = value; }
    public Scene Scene {get => scene; set => scene = value; }
    public SerializableCombatSceneSaveData SerializableCombatSceneSaveData {get => serializableCombatSceneSaveData; set => serializableCombatSceneSaveData = value; }

    public SceneSaveData Deserialize()
    {
        return new SceneSaveData
        {
            Id = Id,
            Scene = Scene,
            SceneType = SceneType,
            CombatSceneSaveData = SerializableCombatSceneSaveData?.Deserialize()
        };
    }
}

[Serializable]
public class SerializableCombatSceneSaveData
{
    [SerializeField] private bool enemiesCleared;
    [SerializeField] private List<SerializableSpawnerSaveData> serializableSpawnerSaveDataList;
    
    public bool EnemiesCleared { get => enemiesCleared; set => enemiesCleared = value; }
    public List<SerializableSpawnerSaveData> SerializableSpawnerSaveDataList { get => serializableSpawnerSaveDataList; set => serializableSpawnerSaveDataList = value; }

    public CombatSceneSaveData Deserialize()
    {
        return new CombatSceneSaveData
        {
            EnemiesCleared = EnemiesCleared,
            SpawnerIdToSpawnedActor = SerializableSpawnerSaveDataList.ToDictionary(
                d => d.Id,
                d => d.ActorType
            )
        };
    }
}

[Serializable]
public class SerializableSpawnerSaveData
{
    [SerializeField] private int id;
    [SerializeField] private ActorType actorType;

    public int Id { get => id; set => id = value; }
    public ActorType ActorType { get => actorType; set => actorType = value; }
}

[Serializable]
public class SerializableSceneTransition
{
    [SerializeField] private int fromId;
    [SerializeField] private List<int> toIds;

    public int FromId { get => fromId; set => fromId = value; }
    public List<int> ToIds { get => toIds; set => toIds = value; }
}
