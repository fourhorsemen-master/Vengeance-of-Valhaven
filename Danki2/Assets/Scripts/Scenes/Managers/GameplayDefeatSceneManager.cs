using UnityEngine;

public class GameplayDefeatSceneManager : MonoBehaviour
{
    private void Start()
    {
        SaveDataManager.Instance.Clear();
        SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
