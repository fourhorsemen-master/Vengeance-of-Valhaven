using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private GameObject abilityTreeMenu = null;

    private bool abilityTreeMenuButtonIsPressed = false;

    protected override void Awake()
    {
        base.Awake();

        abilityTreeMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetAxis("AbilityTreeMenu") > 0 && !this.abilityTreeMenuButtonIsPressed)
        {
            this.abilityTreeMenuButtonIsPressed = true;

            if (abilityTreeMenu.activeInHierarchy)
            {
                abilityTreeMenu.SetActive(false);
                GameController.Instance.GameState = GameState.Playing;
            }
            else
            {
                abilityTreeMenu.SetActive(true);
                GameController.Instance.GameState = GameState.InPlayMenu;
            }
        }
        else if (Input.GetAxis("AbilityTreeMenu") <= 0)
        {
            this.abilityTreeMenuButtonIsPressed = false;
        }
    }
}
