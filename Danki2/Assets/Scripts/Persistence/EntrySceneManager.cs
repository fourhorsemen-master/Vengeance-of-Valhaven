using UnityEngine;

public class EntrySceneManager : MonoBehaviour
{
    private void Start()
    {
        switch (MainMenu.GameType)
        {
            case GameType.Continue:
                GameplaySceneManager.Instance.Continue();
                break;
            case GameType.NewGame:
                GameplaySceneManager.Instance.NewGame();
                break;
        }
    }
}
