using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneManager : PersistentSingleton<GameplaySceneManager>
{
    private static readonly Dictionary<Scene, Scene> nextSceneLookup = new Dictionary<Scene, Scene>
    {
        {Scene.GameplayEntryScene, Scene.GameplayScene1},
        {Scene.GameplayScene1, Scene.GameplayScene2},
        {Scene.GameplayScene2, Scene.GameplayScene3},
        {Scene.GameplayScene3, Scene.GameplayExitScene}
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) LoadNextScene();
        if (Input.GetKeyDown(KeyCode.Escape)) Quit();
    }

    public void LoadStartingScene()
    {
        SceneUtils.LoadScene(PersistenceManager.Instance.SaveData.CurrentScene);
    }

    private void LoadNextScene()
    {
        Scene nextScene = nextSceneLookup[PersistenceManager.Instance.SaveData.CurrentScene];
        PersistenceManager.Instance.SaveData.CurrentScene = nextScene;
        PersistenceManager.Instance.Save();
        SceneUtils.LoadScene(nextScene);
    }

    private void Quit()
    {
        SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
