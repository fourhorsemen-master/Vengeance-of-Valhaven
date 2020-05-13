using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private GameObject abilityTreeMenu = null;

    protected override void Awake()
    {
        base.Awake();

        abilityTreeMenu.SetActive(false);
    }

    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
                abilityTreeMenu.SetActive(gameState == GameState.InAbilityTreeEditor)
        );
    }
}
