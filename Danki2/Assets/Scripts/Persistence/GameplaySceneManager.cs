using System.Collections.Generic;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement.Scene;

public class GameplaySceneManager : Singleton<GameplaySceneManager>
{
    public Scene CurrentScene { get; private set; }

    private static readonly Dictionary<Scene, Scene> nextSceneLookup = new Dictionary<Scene, Scene>
    {
        {Scene.GameplayScene1, Scene.GameplayScene2},
        {Scene.GameplayScene2, Scene.GameplayScene3},
        {Scene.GameplayScene3, Scene.GameplayVictoryScene}
    };

    private void Start()
    {
        CurrentScene = PersistenceManager.Instance.SaveData.CurrentScene;
    }

    private void Update()
    {
        // TODO: hook up controls to real system.
        if (Input.GetKeyDown(KeyCode.Alpha1)) PersistenceManager.Instance.Save(); // when the scene is cleared
        if (Input.GetKeyDown(KeyCode.Alpha2)) LoadNextScene(); // when the next scene is picked to transition to
        if (Input.GetKeyDown(KeyCode.Escape)) SceneUtils.LoadScene(Scene.GameplayExitScene); // to quit
    }

    private void LoadNextScene()
    {
        CurrentScene = nextSceneLookup[CurrentScene];
        PersistenceManager.Instance.Save();
        SceneUtils.LoadScene(CurrentScene);
    }
}
