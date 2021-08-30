using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DankiMenu
{
    [MenuItem("Danki/New Game", priority = 1)]
    private static void NewGame() => RunScene("Assets/Scenes/MetaScenes/EntryScene/EntryScene.unity");

    [MenuItem("Danki/Toggle Dev Mode", priority = 2)]
    private static void ToggleDevMode()
    {
        GameObject metaObject = SceneManager.GetActiveScene()
            .GetRootGameObjects()
            .FirstOrDefault(o => o.name.ToLower() == "meta");

        if (metaObject == null)
        {
            Debug.LogError("Can only toggle dev mode in a gameplay scene");
            return;
        }

        GameObject devManagerObject = metaObject.GetComponentsInChildren<DevPersistenceManager>(true)
            .First().gameObject;

        devManagerObject.SetActive(!devManagerObject.activeInHierarchy);

        Selection.activeGameObject = devManagerObject;

        EditorUtility.SetDirty(devManagerObject);
    }

    [MenuItem("Danki/Scenes/Asset Showcase", priority = 3)]
    private static void OpenAssetShowcaseScene() => OpenScene("Assets/Scenes/AssetShowcase/AssetShowcase.unity");

    [MenuItem("Danki/Scenes/Forest Dens", priority = 4)]
    private static void OpenForestDensScene() => OpenScene("Assets/Scenes/GameplayScenes/Zone1/ForestDens/ForestDensScene.unity");

    private static void OpenScene(string scenePath)
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        EditorSceneManager.OpenScene(scenePath);
    }

    private static void RunScene(string scenePath)
    {
        OpenScene(scenePath);
        EditorApplication.isPlaying = true;
    }
}
