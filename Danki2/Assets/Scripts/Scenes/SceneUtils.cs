using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityScene = UnityEngine.SceneManagement.Scene;

public static class SceneUtils
{
    private static readonly Dictionary<Scene, string> sceneLookup = new Dictionary<Scene, string>
    {
        { Scene.EntryScene, "EntryScene" },
        { Scene.MainMenu, "MainMenu" },
        { Scene.GameplayEntryScene, "GameplayEntryScene" },
        { Scene.GameplayExitScene, "GameplayExitScene" },
        { Scene.GameplayVictoryScene, "GameplayVictoryScene" },
        { Scene.GameplayScene1, "GameplayScene1" },
        { Scene.GameplayScene2, "GameplayScene2" },
        { Scene.GameplayScene3, "GameplayScene3" }
    };

    public static void LoadScene(Scene scene) => SceneManager.LoadScene(sceneLookup[scene]);

    public static Scene FromUnityScene(UnityScene unityScene) => EnumUtils.FromString<Scene>(unityScene.name);
}
