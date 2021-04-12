using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Subject ContinueClickedSubject { get; } = new Subject();

    private void OnEnable() => PauseUtils.Pause();
    
    private void OnDisable() => PauseUtils.Unpause();
    
    public void Continue() => ContinueClickedSubject.Next();
    
    public void Quit() => SceneUtils.LoadScene(Scene.GameplayExitScene);
}
