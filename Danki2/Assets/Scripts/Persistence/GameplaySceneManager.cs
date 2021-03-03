﻿using System.Collections.Generic;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement.Scene;

public class GameplaySceneManager : Singleton<GameplaySceneManager>
{
    public Scene CurrentScene { get; private set; }

    private static readonly ISet<Scene> gameplayScenes = new HashSet<Scene>
    {
        Scene.GameplayScene1,
        Scene.GameplayScene2,
        Scene.GameplayScene3
    };

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
        if (Input.GetKeyDown(KeyCode.Alpha1)) PersistenceManager.Instance.Save(); // when the scene is cleared
        if (Input.GetKeyDown(KeyCode.Alpha2)) LoadNextScene(); // when the next scene is picked to transition to
    }

    private void LoadNextScene()
    {
        CurrentScene = nextSceneLookup[CurrentScene];
        PersistenceManager.Instance.Save();
        SceneUtils.LoadScene(CurrentScene);
    }
}
