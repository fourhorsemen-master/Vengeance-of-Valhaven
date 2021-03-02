using UnityEngine;

public class ExitSceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneUtils.LoadScene(Scene.MainMenu);
    }
}
