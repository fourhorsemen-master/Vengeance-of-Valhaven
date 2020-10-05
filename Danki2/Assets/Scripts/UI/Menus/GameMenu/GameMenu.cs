using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            gameObject.SetActive(gameState == GameState.InGameMenu);
        });
    }
}
