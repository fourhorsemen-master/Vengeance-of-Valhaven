using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles combat rooms. If we are not in a combat room, then this class does nothing.
///
/// Spawns enemies at the correct spawners and marks the room as completed once all enemies are defeated.
/// </summary>
public class CombatRoomManager : Singleton<CombatRoomManager>
{
    private const int SecondWindHealAmount = 1;

    private const float BountyHunterCurrencyMultiplier = 1.3f;

    public bool EnemiesCleared { get; private set; } = false;
    public Subject EnemiesClearedSubject { get; }  = new Subject();
    public bool InCombatRoom { get; private set; }

    public int TotalEnemyCount { get; private set; } = 0;
    public int DeadEnemyCount { get; private set; } = 0;

    private const float SiphonCurrencyMultiplier = 1.5f;

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

        ActorCache.Instance.ActorRegisteredSubject
            .Where(x => x.CompareTag(Tag.Enemy))
            .Subscribe(x => TrackEnemy((Enemy)x));

        SpawnEnemies();
    }

    private void TrackEnemy(Enemy enemy)
    {
        TotalEnemyCount++;

        enemy.DeathSubject.Subscribe(deathData =>
        {
            if (ActorCache.Instance.Player.RuneManager.HasRune(Rune.SecondWind))
            {
                ActorCache.Instance.Player.HealthManager.ReceiveHeal(SecondWindHealAmount);
            }

            YieldCurrency(enemy, deathData);

            if (ActorCache.Instance.Cache.All(x => !x.Actor.CompareTag(Tag.Enemy)))
            {
                CompleteRoom();
            }

            DeadEnemyCount++;
            if (DeadEnemyCount != TotalEnemyCount) return;
        });
    }

    private void SpawnEnemies()
    {
        CombatRoomSaveData combatRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomNode.CombatRoomSaveData;
        
        Dictionary<int, EnemySpawner> spawnerLookup = FindObjectsOfType<EnemySpawner>().ToDictionary(s => s.Id);

        foreach (int spawnerId in combatRoomSaveData.SpawnerIdToSpawnedActor.Keys)
        {
            EnemySpawner spawner = spawnerLookup[spawnerId];
            ActorType actorType = combatRoomSaveData.SpawnerIdToSpawnedActor[spawnerId];
            spawner.Spawn(actorType);
        }
    }

    private void YieldCurrency(Enemy enemy, DeathData deathData)
    {
        float baseValue = CurrencyLookup.Instance.EnemyCurrencyValueLookup[enemy.Type];

        int siphonCount = deathData.Empowerments.Count(e => e == Empowerment.Siphon);
        float siphonMultiplier = 1 + siphonCount * (SiphonCurrencyMultiplier - 1);

        float bountyHunterMultiplier = ActorCache.Instance.Player.RuneManager.HasRune(Rune.BountyHunter)
            ? BountyHunterCurrencyMultiplier
            : 1;

        int currencyValue = Mathf.CeilToInt(baseValue * siphonMultiplier * BountyHunterCurrencyMultiplier);

        ActorCache.Instance.Player.CurrencyManager.AddCurrency(currencyValue);
        CurrencyCollectionVisual.Create(enemy.Centre, currencyValue);
    }

    private void CompleteRoom()
    {
        EnemiesCleared = true;
        EnemiesClearedSubject.Next();
        PersistenceManager.Instance.Save();
    }
}
