using UnityEngine;

public class GameplayExitSceneManager : MonoBehaviour
{
    private void Start()
    {
        PersistenceManager.Instance.Destroy();
        AbilityLookup.Instance.Destroy();
        SceneUtils.LoadScene(Scene.MainMenu);
    }
}
