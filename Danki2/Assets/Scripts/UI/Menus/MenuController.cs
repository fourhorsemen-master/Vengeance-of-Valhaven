using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : Singleton<MenuController>
{
    [SerializeField]
    public List<MenuData> menuData = new List<MenuData>();
    
    private void Start()
    {
        Dictionary<GameplayState, GameObject> menuLookup = menuData.ToDictionary(d => d.GameplayState, d => d.Menu);

        GameplayStateController.Instance.GameStateTransitionSubject.Subscribe(newGameplayState =>
        {
            foreach (GameplayState gameplayState in menuLookup.Keys)
            {
                if (!menuLookup.ContainsKey(gameplayState)) return;
                menuLookup[gameplayState].SetActive(gameplayState == newGameplayState);
            }
        });
    }
}
