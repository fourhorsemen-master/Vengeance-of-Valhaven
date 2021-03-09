using UnityEngine;

public class GameplayExitSceneManager : MonoBehaviour
{
    private void Start()
    {
        PersistenceManager.Instance.Destroy();
        SceneUtils.LoadScene(Scene.MainMenu);
    }
}
