using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MapGenerationLookupTest
{
    [UnityTest]
    public IEnumerator TestScenesAllHaveRequiredSpawners()
    {
        List<string> combatScenes = GetCombatScenes();
        
        TestUtils.InstantiatePrefab<DevPersistenceManager>();
        Object.DontDestroyOnLoad(DevPersistenceManager.Instance);
        
        combatScenes.ForEach(combatScene =>
        {
            SceneManager.LoadScene(combatScene);
        });

        TestUtils.LoadEmptyScene();
        Object.Destroy(DevPersistenceManager.Instance.gameObject);

        yield return null;
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
}
