using UnityEditor;
using UnityEditor.SceneManagement;

public static class DankiMenu
{
    [MenuItem("Danki/Run Entry Scene")]
    private static void RunEntryScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MetaScenes/EntryScene/EntryScene.unity");
        
        EditorApplication.isPlaying = true;
    }
}