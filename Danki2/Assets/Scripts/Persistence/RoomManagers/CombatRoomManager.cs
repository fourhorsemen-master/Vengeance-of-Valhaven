using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles combat rooms. If we are not in a combat room, then this class does nothing.
///
/// Spawns enemies at the correct spawners and marks the room as completed once all enemies are defeated.
/// </summary>
public class CombatRoomManager : Singleton<CombatRoomManager>
{
    public bool EnemiesCleared { get; private set; } = false;
    
    private void Start()
    {
        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;

        if (roomSaveData.RoomType != RoomType.Combat) return;
        
        CombatRoomSaveData combatRoomSaveData = roomSaveData.CombatRoomSaveData;

        Subject roomClearedSubject = new Subject();
        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(roomClearedSubject);

        if (combatRoomSaveData.EnemiesCleared)
        {
            roomClearedSubject.Next();
            return;
        }

        Dictionary<int, Spawner> spawnerLookup = FindObjectsOfType<Spawner>().ToDictionary(s => s.Id);

        List<Actor> enemies = new List<Actor>();
        foreach (int spawnerId in combatRoomSaveData.SpawnerIdToSpawnedActor.Keys)
        {
            Spawner spawner = spawnerLookup[spawnerId];
            ActorType actorType = combatRoomSaveData.SpawnerIdToSpawnedActor[spawnerId];
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
