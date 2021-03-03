using UnityEngine;

public class GameplayVictorySceneManager : MonoBehaviour
{
    private void Start()
    {
        SaveDataManager.Instance.Clear();
        SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
