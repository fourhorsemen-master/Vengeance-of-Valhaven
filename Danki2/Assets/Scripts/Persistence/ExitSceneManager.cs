using UnityEngine;

public class ExitSceneManager : MonoBehaviour
{
    private void Start()
    {
        GameplaySceneManager.Instance.Destroy();
        PersistenceManager.Instance.Destroy();

        SceneUtils.LoadScene(Scene.MainMenu);
    }
}
