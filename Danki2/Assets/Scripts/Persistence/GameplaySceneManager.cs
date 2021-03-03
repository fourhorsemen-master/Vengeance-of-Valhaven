using UnityEngine;

public class GameplaySceneManager : Singleton<GameplaySceneManager>
{
    private static Scene NextScene = Scene.GameplayScene1;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) LoadNextScene();
        if (Input.GetKeyDown(KeyCode.Escape)) SaveAndQuit();
    }

    public void Continue() => LoadNextScene();

    public void NewGame()
    {
        NextScene = Scene.GameplayScene1;
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        switch (NextScene)
        {
            case Scene.GameplayScene1:
                NextScene = Scene.GameplayScene2;
                SceneUtils.LoadScene(Scene.GameplayScene1);
                break;
            case Scene.GameplayScene2:
                NextScene = Scene.GameplayScene3;
                SceneUtils.LoadScene(Scene.GameplayScene2);
                break;
            case Scene.GameplayScene3:
                NextScene = Scene.ExitScene;
                SceneUtils.LoadScene(Scene.GameplayScene3);
                break;
            case Scene.ExitScene:
                NextScene = Scene.GameplayScene1;
                SceneUtils.LoadScene(Scene.ExitScene);
                break;
        }
    }

    private void SaveAndQuit()
    {
        switch (NextScene)
        {
            case Scene.GameplayScene1:
                NextScene = Scene.GameplayScene1;
                SceneUtils.LoadScene(Scene.ExitScene);
                break;
            case Scene.GameplayScene2:
                NextScene = Scene.GameplayScene1;
                SceneUtils.LoadScene(Scene.ExitScene);
                break;
            case Scene.GameplayScene3:
                NextScene = Scene.GameplayScene2;
                SceneUtils.LoadScene(Scene.ExitScene);
                break;
            case Scene.ExitScene:
                NextScene = Scene.GameplayScene3;
                SceneUtils.LoadScene(Scene.ExitScene);
                break;
        }
    }
}
