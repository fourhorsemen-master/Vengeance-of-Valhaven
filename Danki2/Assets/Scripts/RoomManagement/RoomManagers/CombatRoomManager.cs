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
    public Subject EnemiesClearedSubject { get; }  = new Subject();
    public bool InCombatRoom { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        InCombatRoom = roomSaveData.RoomType == RoomType.Combat || roomSaveData.RoomType == RoomType.Boss;

        if (!InCombatRoom) return;

        EnemiesCleared = roomSaveData.CombatRoomSaveData.EnemiesCleared;
    }

    private void Start()
    {
        if (!InCombatRoom) return;

        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(EnemiesClearedSubject);

        if (EnemiesCleared)
        {
            EnemiesCleared = true;
            EnemiesClearedSubject.Next();
            return;
        }

        List<Enemy> enemies = SpawnEnemies();
        TrackEnemyDeaths(enemies);
    }

    private List<Enemy> SpawnEnemies()
    {
        CombatRoomSaveData combatRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.CombatRoomSaveData;
        
        Dictionary<int, EnemySpawner> spawnerLookup = FindObjectsOfType<EnemySpawner>().ToDictionary(s => s.Id);

        List<Enemy> enemies = new List<Enemy>();
        foreach (int spawnerId in combatRoomSaveData.SpawnerIdToSpawnedActor.Keys)
        {
            EnemySpawner spawner = spawnerLookup[spawnerId];
            ActorType actorType = combatRoomSaveData.SpawnerIdToSpawnedActor[spawnerId];
            enemies.Add(spawner.Spawn(actorType));
        }

        return enemies;
    }

    private void TrackEnemyDeaths(List<Enemy> enemies)
    {
        int enemyCount = enemies.Count;
        int deadEnemyCount = 0;
        enemies.ForEach(e => e.DeathSubject.Subscribe(() =>
        {
            deadEnemyCount++;
            if (deadEnemyCount != enemyCount) return;

            EnemiesCleared = true;
            EnemiesClearedSubject.Next();
            PersistenceManager.Instance.Save();
        }));
    }
}
