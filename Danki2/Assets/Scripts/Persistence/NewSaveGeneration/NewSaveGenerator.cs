using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NewSaveGenerator
{
    private const int SaveDataVersion = 1;

    private const int MinRoomExits = 1;
    private const int MaxRoomExits = 3;
    private const int MinRoomDepth = 2;
    private const int MaxRoomDepth = 3;

    private static Dictionary<Pole, Pole> ExitDirectionToEntranceDirectionLookup = new Dictionary<Pole, Pole>
    {
        [Pole.North] = Pole.South,
        [Pole.East] = Pole.West,
        [Pole.South] = Pole.North,
        [Pole.West] = Pole.East,
    };
    
    /// <summary>
    /// Generates a new save data object which can be used to start a new game.
    /// </summary>
    /// <param name="seed"> An optional seed to generate the save with </param>
    public static SaveData Generate(int seed = -1)
    {
        if (seed == -1) seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);

        RoomLayoutNode rootNode = new RoomLayoutNode();
        RecursivelyGenerateChildren(rootNode, 1);
        SetIds(rootNode);

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
            RoomSaveDataLookup = GenerateNewRoomSaveDataLookup(rootNode)
        };
    }

    private static void RecursivelyGenerateChildren(RoomLayoutNode node, int currentDepth)
    {
        int numberOfChildren = Random.Range(MinRoomExits, MaxRoomExits + 1);

        for (int i = 0; i < numberOfChildren; i++)
        {
            RoomLayoutNode childNode = new RoomLayoutNode();
            node.Children.Add(childNode);

            int nextDepth = currentDepth + 1;
            if (ShouldGenerateChildren(childNode, nextDepth)) RecursivelyGenerateChildren(childNode, nextDepth);
        }
    }

    /// <summary>
    /// A weighted probability that decides whether a node should have children based on its depth in the tree.
    /// This works out so that every depth is generated with equal probability.
    /// </summary>
    private static bool ShouldGenerateChildren(RoomLayoutNode node, int nodeDepth)
    {
        if (nodeDepth < MinRoomDepth) return true;
        if (nodeDepth >= MaxRoomDepth) return false;
        return Random.value < 1 / (MaxRoomDepth - nodeDepth + 1);
    }

    private static void SetIds(RoomLayoutNode rootNode)
    {
        int currentId = 0;
        rootNode.IterateDown(n =>
        {
            n.Id = currentId;
            currentId++;
        });
    }
    
    private static Dictionary<int, RoomSaveData> GenerateNewRoomSaveDataLookup(RoomLayoutNode rootNode)
    {
        Dictionary<int, RoomSaveData> roomSaveDataLookup = new Dictionary<int, RoomSaveData>();
        RecursivelyGenerateRoomSaveData(rootNode, Pole.South, roomSaveDataLookup);
        return roomSaveDataLookup;
    }

    private static void RecursivelyGenerateRoomSaveData(
        RoomLayoutNode currentNode,
        Pole entranceDirection,
        Dictionary<int, RoomSaveData> roomSaveDataLookup
    )
    {
        RoomSaveData roomSaveData = new RoomSaveData();
        roomSaveData.Id = currentNode.Id;

        int numberOfExits = currentNode.Children.Count;
        Scene selectedScene = RandomUtils.Choice(GetUsableScenes(entranceDirection, numberOfExits));
        roomSaveData.Scene = selectedScene;

        roomSaveData.RoomType = RoomType.Combat;

        roomSaveData.CombatRoomSaveData = new CombatRoomSaveData()
        {
            EnemiesCleared = false,
            SpawnerIdToSpawnedActor = GetValidRandomSpawnedActors(selectedScene)
        };
        
        Pole cameraOrientation = GetValidRandomCameraOrientation(selectedScene, entranceDirection, numberOfExits);

        Dictionary<int, int> roomTransitionerIdToNextRoomId = new Dictionary<int, int>();
        List<int> exitIds = GetValidRandomExitIds(selectedScene, entranceDirection, cameraOrientation, numberOfExits);

        for (int i = 0; i < exitIds.Count; i++)
        {
            roomTransitionerIdToNextRoomId[exitIds[i]] = currentNode.Children[i].Id;
        }

        roomSaveData.RoomTransitionerIdToNextRoomId = roomTransitionerIdToNextRoomId;
        
        
        roomSaveData.ModuleSeed = Random.Range(0, int.MaxValue);
        
        roomSaveData.CameraOrientation = cameraOrientation;

        roomSaveData.PlayerSpawnerId = GetValidRandomEntranceId(selectedScene, entranceDirection, cameraOrientation);

        roomSaveDataLookup[roomSaveData.Id] = roomSaveData;

        for (int i = 0; i < currentNode.Children.Count; i++)
        {
            RecursivelyGenerateRoomSaveData(
                currentNode.Children[i],
                ExitDirectionToEntranceDirectionLookup[SceneLookup.Instance.sceneDataLookup[selectedScene].GameplaySceneData.ExitData.First(d => d.Id == exitIds[i]).Side],
                roomSaveDataLookup
            );
        }
    }

    private static List<Scene> GetUsableScenes(Pole entranceDirection, int numberOfExits)
    {
        // TODO: Select appropriate scenes.
        return ListUtils.Singleton(Scene.RandomisedScene);
    }

    private static Pole GetValidRandomCameraOrientation(Scene scene, Pole entranceDirection, int numberOfExits)
    {
        return Pole.North;
    }

    private static int GetValidRandomEntranceId(Scene scene, Pole entranceDirection, Pole cameraOrientation)
    {
        return 0;
    }

    private static List<int> GetValidRandomExitIds(
        Scene scene,
        Pole entranceDirection,
        Pole cameraOrientation,
        int exitCount
    )
    {
        List<int> list = new List<int>();
        
        if (exitCount == 1) list.Add(0);

        if (exitCount == 2)
        {
            list.Add(0);
            list.Add(1);
        }

        if (exitCount == 3)
        {
            list.Add(0);
            list.Add(1);
            list.Add(2);
        }
        
        return list;
    }

    private static Dictionary<int, ActorType> GetValidRandomSpawnedActors(Scene scene)
    {
        return new Dictionary<int, ActorType>{[0] = ActorType.Wolf};
    }
    
    private static Dictionary<int, RoomSaveData> GenerateNewRoomSaveDataLookup()
    {
        return new Dictionary<int, RoomSaveData>
        {
            [0] = new RoomSaveData
            {
                Id = 0,
                Scene = Scene.RandomisedScene,
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
                Scene = Scene.RandomisedScene,
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
                Scene = Scene.RandomisedScene,
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
                Scene = Scene.RandomisedScene,
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
                Scene = Scene.RandomisedScene,
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
                Scene = Scene.RandomisedScene,
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
                Scene = Scene.RandomisedScene,
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
}
