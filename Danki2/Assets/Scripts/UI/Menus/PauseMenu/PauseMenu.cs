using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        PauseManager.Pause();
    }

    private void OnDisable()
    {
        PauseManager.UnPause();
    }

    public void Continue()
    {
        GameplayStateController.Instance.GameplayState = GameplayState.Playing;
    }

    public void Quit()
    {
        SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
