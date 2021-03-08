﻿using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles combat scenes. If we are not in a combat scene, then this class does nothing.
///
/// Spawns enemies at the correct spawners and marks the room as completed once all enemies are defeated.
/// </summary>
public class CombatSceneManager : Singleton<CombatSceneManager>
{
    public bool EnemiesCleared { get; private set; } = false;
    
    private void Start()
    {
        int sceneId = PersistenceManager.Instance.SaveData.CurrentSceneId;
        SceneSaveData sceneSaveData = PersistenceManager.Instance.SaveData.SceneSaveDataLookup[sceneId];

        if (sceneSaveData.SceneType != SceneType.Combat) return;
        
        CombatSceneSaveData combatSceneSaveData = sceneSaveData.CombatSceneSaveData;

        Subject roomClearedSubject = new Subject();
        GameplaySceneTransitionManager.Instance.RegisterCanTransitionSubject(roomClearedSubject);

        if (combatSceneSaveData.EnemiesCleared)
        {
            roomClearedSubject.Next();
            return;
        }

        Dictionary<int, Spawner> spawnerLookup = FindObjectsOfType<Spawner>().ToDictionary(s => s.Id);

        List<Actor> enemies = new List<Actor>();
        foreach (int spawnerId in combatSceneSaveData.SpawnerIdToSpawnedActor.Keys)
        {
            Spawner spawner = spawnerLookup[spawnerId];
            ActorType actorType = combatSceneSaveData.SpawnerIdToSpawnedActor[spawnerId];
            enemies.Add(spawner.Spawn(actorType));
        }

        int enemyCount = enemies.Count;
        int deadEnemyCount = 0;
        enemies.ForEach(e => e.DeathSubject.Subscribe(() =>
        {
            deadEnemyCount++;
            if (deadEnemyCount != enemyCount) return;

            EnemiesCleared = true;
            roomClearedSubject.Next();
            PersistenceManager.Instance.Save();
        }));
    }
}
