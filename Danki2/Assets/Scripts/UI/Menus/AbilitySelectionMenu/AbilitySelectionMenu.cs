using UnityEngine;

public class AbilitySelectionMenu : MonoBehaviour
{
    private void Start()
    {
        GameplayStateController.Instance.GameStateTransitionSubject.Subscribe(gameplayState =>
        {
            gameObject.SetActive(gameplayState == GameplayState.InAbilitySelection);
        });
    }
}
