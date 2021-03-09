using UnityEngine;

public class GameplayVictorySceneManager : MonoBehaviour
{
    public void MainMenu()
    {
        SaveDataManager.Instance.Clear();
        SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
