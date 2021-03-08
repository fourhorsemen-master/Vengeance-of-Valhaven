using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
