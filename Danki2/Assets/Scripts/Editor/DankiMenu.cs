using UnityEditor;
using UnityEditor.SceneManagement;

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
}