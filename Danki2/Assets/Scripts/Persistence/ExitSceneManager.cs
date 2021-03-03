using UnityEngine;

public class ExitSceneManager : MonoBehaviour
{
    private void Start()
    {
        GameplaySceneManager.Instance.Destroy();
        SceneUtils.LoadScene(Scene.MainMenu);
    }
}
