using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneUtils
{
    private static readonly Dictionary<Scene, string> sceneLookup = new Dictionary<Scene, string>
    {
        { Scene.MainMenu, "MainMenu" },
        { Scene.EntryScene, "EntryScene" },
        { Scene.ExitScene, "ExitScene" },
        { Scene.GameplayScene1, "GameplayScene1" },
        { Scene.GameplayScene2, "GameplayScene2" },
        { Scene.GameplayScene3, "GameplayScene3" }
    };

    public static void LoadScene(Scene scene) => SceneManager.LoadScene(sceneLookup[scene]);
}
