using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DankiMenu
{
    [MenuItem("Danki/Entry Scene")]
    private static void RunEntryScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MetaScenes/EntryScene/EntryScene.unity");
        
        EditorApplication.isPlaying = true;
    }

    [MenuItem("Danki/Asset Showcase")]
    private static void RunAssetShowcase()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/AssetShowcase/AssetShowcase.unity");

        EditorApplication.isPlaying = true;
    }

    [MenuItem("Danki/Toggle Dev Mode")]
    private static void ToggleDevMode(MenuCommand menuCommand)
    {
        var metaObject = SceneManager.GetActiveScene()
            .GetRootGameObjects()
            .FirstOrDefault(o => o.name.ToLower() == "meta");

        if (metaObject == null)
        {
            Debug.LogError("Can only toggle dev mode in a gameplay scene");
            return;
        }

        var devManagerObject = metaObject.GetComponentsInChildren<DevPersistenceManager>(true)
            .First().gameObject;

        devManagerObject.SetActive(!devManagerObject.activeInHierarchy);

        Selection.activeGameObject = devManagerObject;
    }
}