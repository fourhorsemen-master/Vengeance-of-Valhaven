using UnityEditor;

[InitializeOnLoad]
public static class DankiMenu
{
    [MenuItem("Danki/Run Entry Scene")]
    private static void RunEntryScene()
    {
        EditorApplication.OpenScene("Assets/Scenes/MetaScenes/EntryScene/EntryScene.unity");
        
        EditorApplication.isPlaying = true;
    }
}