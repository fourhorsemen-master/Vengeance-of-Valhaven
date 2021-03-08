using System.Collections.Generic;
using System.Linq;

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