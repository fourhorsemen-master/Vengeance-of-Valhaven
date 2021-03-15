using System.Collections.Generic;
using UnityEngine;

public static class NewSaveGenerator
{
    private const int SaveDataVersion = 1;

    /// <summary>
    /// Generates a new save data object which can be used to start a new game.
    /// </summary>
    public static SaveData Generate()
    {
        return new SaveData
        {
            Version = SaveDataVersion,
            PlayerHealth = 20,
            AbilityTree = AbilityTreeFactory.CreateTree(
                new EnumDictionary<AbilityReference, int>(3),
                AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
                AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
            ),
            CurrentRoomId = 0,
            DefeatRoomId = 8,
            RoomSaveDataLookup = GenerateNewRoomSaveDataLookup(),
            RoomTransitions = GenerateNewRoomTransitions()
        };
    }

    private static Dictionary<int, RoomSaveData> GenerateNewRoomSaveDataLookup()
    {
        return new Dictionary<int, RoomSaveData>
        {
            [0] = new RoomSaveData
            {
                Id = 0,
                Scene = Scene.GameplayScene4,
                RoomType = RoomType.Combat,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Wolf
                    }
                },
                RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                {
                    [0] = 1
                },
                ModuleSeed = Random.Range(0, int.MaxValue),
                CameraOrientation = Pole.West,
                PlayerSpawnerId = 1
            },
            [1] = new RoomSaveData
            {
                Id = 1,
                Scene = Scene.GameplayScene4,
                RoomType = RoomType.Combat,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Wolf
                    }
                },
                RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                {
                    [0] = 2,
                    [1] = 4,
                    [2] = 5
                },
                ModuleSeed = Random.Range(0, int.MaxValue),
                CameraOrientation = Pole.North,
                PlayerSpawnerId = 0
            },
            [2] = new RoomSaveData
            {
                Id = 2,
                Scene = Scene.GameplayScene4,
                RoomType = RoomType.Combat,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Wolf,
                        [1] = ActorType.Wolf
                    }
                },
                RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                {
                    [1] = 3
                },
                ModuleSeed = Random.Range(0, int.MaxValue),
                CameraOrientation = Pole.East,
                PlayerSpawnerId = 0
            },
            [3] = new RoomSaveData
            {
                Id = 3,
                Scene = Scene.GameplayScene4,
                RoomType = RoomType.Combat,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Bear,
                        [1] = ActorType.Bear
                    }
                },
                RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                {
                    [1] = 7
                },
                ModuleSeed = Random.Range(0, int.MaxValue),
                CameraOrientation = Pole.North,
                PlayerSpawnerId = 1
            },
            [4] = new RoomSaveData
            {
                Id = 4,
                Scene = Scene.GameplayScene4,
                RoomType = RoomType.Combat,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Bear
                    }
                },
                RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                {
                    [0] = 7
                },
                ModuleSeed = Random.Range(0, int.MaxValue),
                CameraOrientation = Pole.West,
                PlayerSpawnerId = 1
            },
            [5] = new RoomSaveData
            {
                Id = 5,
                Scene = Scene.GameplayScene4,
                RoomType = RoomType.Combat,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Wolf,
                        [1] = ActorType.Wolf
                    }
                },
                RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                {
                    [0] = 6
                },
                ModuleSeed = Random.Range(0, int.MaxValue),
                CameraOrientation = Pole.West,
                PlayerSpawnerId = 0
            },
            [6] = new RoomSaveData
            {
                Id = 6,
                Scene = Scene.GameplayScene4,
                RoomType = RoomType.Combat,
                CombatRoomSaveData = new CombatRoomSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Bear,
                        [1] = ActorType.Wolf
                    }
                },
                RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                {
                    [1] = 7
                },
                ModuleSeed = Random.Range(0, int.MaxValue),
                CameraOrientation = Pole.North,
                PlayerSpawnerId = 0
            },
            [7] = new RoomSaveData
            {
                Id = 7,
                Scene = Scene.GameplayVictoryScene,
                RoomType = RoomType.Victory
            },
            [8] = new RoomSaveData
            {
                Id = 8,
                Scene = Scene.GameplayDefeatScene,
                RoomType = RoomType.Defeat
            }
        };
    }

    private static Dictionary<int, List<int>> GenerateNewRoomTransitions()
    {
        return new Dictionary<int, List<int>>
        {
            [0] = new List<int> {1},
            [1] = new List<int> {2, 4, 5},
            [2] = new List<int> {3},
            [3] = new List<int> {7},
            [4] = new List<int> {7},
            [5] = new List<int> {6},
            [6] = new List<int> {7}
        };
    }
}
