using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityScene = UnityEngine.SceneManagement.Scene;

public class GameplaySceneManager : NotDestroyedOnLoadSingleton<GameplaySceneManager>
{
    public Subject<Scene> GameplaySceneLoadedSubject { get; } = new Subject<Scene>(); 

    private static readonly ISet<Scene> gameplayScenes = new HashSet<Scene>
    {
        Scene.GameplayScene1,
        Scene.GameplayScene2,
        Scene.GameplayScene3
    };

    private static readonly Dictionary<Scene, Scene> nextSceneLookup = new Dictionary<Scene, Scene>
    {
        {Scene.GameplayEntryScene, Scene.GameplayScene1},
        {Scene.GameplayScene1, Scene.GameplayScene2},
        {Scene.GameplayScene2, Scene.GameplayScene3},
        {Scene.GameplayScene3, Scene.GameplayExitScene}
    };

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) LoadNextScene();
    }

    public void LoadStartingScene()
    {
        SceneUtils.LoadScene(PersistenceManager.Instance.SaveData.CurrentScene);
    }

    private void LoadNextScene()
    {
        Scene nextScene = nextSceneLookup[PersistenceManager.Instance.SaveData.CurrentScene];

        if (nextScene == Scene.GameplayExitScene)
        {
            SaveDataManager.Instance.Clear();
        }
        else
        {
            PersistenceManager.Instance.SaveData.CurrentScene = nextScene;
            PersistenceManager.Instance.Save();
        }

        SceneUtils.LoadScene(nextScene);
    }

    private void OnSceneLoaded(UnityScene unityScene, LoadSceneMode mode)
    {
        Scene scene = SceneUtils.FromUnityScene(unityScene);
        if (gameplayScenes.Contains(scene)) GameplaySceneLoadedSubject.Next(scene);
    }
}
