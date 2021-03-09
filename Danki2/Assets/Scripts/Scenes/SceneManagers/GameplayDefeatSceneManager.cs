using UnityEngine;

public class GameplayDefeatSceneManager : MonoBehaviour
{
    public void MainMenu()
    {
        SaveDataManager.Instance.Clear();
        SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
