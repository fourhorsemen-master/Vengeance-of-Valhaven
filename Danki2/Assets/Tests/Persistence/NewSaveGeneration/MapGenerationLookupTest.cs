using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MapGenerationLookupTest
{
    [UnityTest]
    public IEnumerator TestScenesAllHaveRequiredSpawners()
    {
        List<string> combatScenes = GetCombatScenes();
        int requiredSpawners = GetRequiredSpawners();
        
        TestUtils.InstantiatePrefab<DevPersistenceManager>();
        DevPersistenceManager.Instance.DontDestroyOnLoad();
        
        foreach (string combatScene in combatScenes)
        {
            SceneManager.LoadScene(combatScene);
            yield return null;
            AssertCorrectSpawners(requiredSpawners, combatScene);
        }

        TestUtils.LoadEmptyScene();
        yield return null;
        DevPersistenceManager.Instance.Destroy();
    }

    private List<string> GetCombatScenes()
    {
        TestUtils.InstantiatePrefab<SceneLookup>();

        List<string> combatScenes = SceneLookup.Instance.sceneDataLookup.Values
            .Where(sceneData => sceneData.SceneType == SceneType.Gameplay)
            .Where(sceneData => sceneData.GameplaySceneData.RoomTypes.Contains(RoomType.Combat))
            .Select(sceneData => sceneData.FileName)
            .ToList();
        
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
