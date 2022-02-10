using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MapGenerationLookupTest : PlayModeTestBase
{
    [UnityTest] public IEnumerator TestZone1ScenesHaveRequiredSpawners() =>
        TestZoneScenesAllHaveRequiredSpawners(Zone.Zone1);

    [UnityTest] public IEnumerator TestZone2ScenesHaveRequiredSpawners() =>
        TestZoneScenesAllHaveRequiredSpawners(Zone.Zone2);

    [UnityTest] public IEnumerator TestZone3ScenesHaveRequiredSpawners() =>
        TestZoneScenesAllHaveRequiredSpawners(Zone.Zone3);

    private IEnumerator TestZoneScenesAllHaveRequiredSpawners(Zone zone)
    {
        EnumDictionary<Zone, List<string>> combatScenes = GetCombatScenes();
        yield return null;
        int requiredSpawners = GetRequiredSpawners();
        yield return null;

        var test = TestUtils.InstantiatePrefab<DevPersistenceManager>();
        DevPersistenceManager.Instance.DontDestroyOnLoad();

        test.zone = zone;

        foreach (string combatScene in combatScenes[zone])
        {
            SceneManager.LoadScene(combatScene);
            yield return null;
            AssertCorrectSpawners(requiredSpawners, combatScene);
        }

        DevPersistenceManager.Instance.Destroy();
        yield return null;
    }

    private EnumDictionary<Zone, List<string>> GetCombatScenes()
    {
        TestUtils.InstantiatePrefab<SceneLookup>();

        EnumDictionary<Zone, List<string>> combatScenes = new EnumDictionary<Zone, List<string>>(() => new List<string>());

        SceneLookup.Instance.sceneDataLookup.Values
            .Where(sceneData => sceneData.SceneType == SceneType.Gameplay)
            .Where(sceneData => sceneData.GameplaySceneData.RoomTypes.Contains(RoomType.Combat))
            .ToList()
            .ForEach(sceneData => sceneData.GameplaySceneData.Zones.ForEach(x => combatScenes[x].Add(sceneData.FileName)));
        
        SceneLookup.Instance.Destroy();
        return combatScenes;
    }

    private int GetRequiredSpawners()
    {
        TestUtils.InstantiatePrefab<MapGenerationLookup>();
        int requiredSpawners = MapGenerationLookup.Instance.RequiredSpawners;
        MapGenerationLookup.Instance.Destroy();
        return requiredSpawners;
    }

    private void AssertCorrectSpawners(int requiredSpawners, string scene)
    {
        EnemySpawner[] enemySpawners = Object.FindObjectsOfType<EnemySpawner>();

        Assert.AreEqual(
            enemySpawners.Length,
            requiredSpawners,
            $"{scene} does not have exactly {requiredSpawners} spawners."
        );

        for (int i = 0; i < requiredSpawners; i++)
        {
            Assert.True(
                enemySpawners.Any(s => s.Id == i),
                $"{scene} does not have the correct IDs set on its spawners."
            );
        }
    }
}
