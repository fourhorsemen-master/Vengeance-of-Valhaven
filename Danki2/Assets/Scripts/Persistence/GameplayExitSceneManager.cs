using UnityEngine;

public class GameplayExitSceneManager : MonoBehaviour
{
    private void Start()
    {
        GameplaySceneManager.Instance.Destroy();
        PersistenceManager.Instance.Destroy();

        SceneUtils.LoadScene(Scene.MainMenu);
    }
}
