using UnityEngine;

public class GameplayEntrySceneManager : MonoBehaviour
{
    private void Start()
    {
        GameplaySceneManager.Instance.LoadStartingScene();
    }
}
