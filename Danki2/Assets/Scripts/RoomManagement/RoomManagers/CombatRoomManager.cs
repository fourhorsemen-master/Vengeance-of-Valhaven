using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles combat rooms. If we are not in a combat room, then this class does nothing.
///
/// Spawns enemies at the correct spawners and marks the room as completed once all enemies are defeated.
/// </summary>
public class CombatRoomManager : Singleton<CombatRoomManager>
{
    private const int FirstAidHealAmount = 3;
    
    public bool EnemiesCleared { get; private set; } = false;
    public Subject EnemiesClearedSubject { get; }  = new Subject();
    public bool InCombatRoom { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        RoomNode roomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;
        InCombatRoom = roomNode.RoomType == RoomType.Combat || roomNode.RoomType == RoomType.Boss;

        if (!InCombatRoom) return;

        EnemiesCleared = roomNode.CombatRoomSaveData.EnemiesCleared;
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
        CombatRoomSaveData combatRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomNode.CombatRoomSaveData;
        
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

            if (ActorCache.Instance.Player.RuneManager.HasRune(Rune.FirstAid))
            {
                ActorCache.Instance.Player.HealthManager.ReceiveHeal(FirstAidHealAmount);
            }
            
            EnemiesCleared = true;
            EnemiesClearedSubject.Next();
            PersistenceManager.Instance.Save();
        }));
    }
}
