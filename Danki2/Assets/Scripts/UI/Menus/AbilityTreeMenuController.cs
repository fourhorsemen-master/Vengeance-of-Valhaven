using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeMenuController : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private GameObject abilityTreeListContent = null;

    [SerializeField]
    private AbilityPanel abilityListingPanelPrefab = null;

    void Start()
    {
        player = RoomManager.Instance.Player;

        PopulateAbilityList();
    }

    private void PopulateAbilityList()
    {
        //destroy all current panels
        foreach (Transform child in abilityTreeListContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<AbilityReference, int> item in player.AbilityInventory)
        {
            if (item.Value > 0)
            {
                AbilityPanel abilityPanel = Instantiate(abilityListingPanelPrefab, Vector3.zero, Quaternion.identity);
                abilityPanel.transform.SetParent(abilityTreeListContent.transform, false);

                abilityPanel.Initialise(item.Key, item.Value);
            }
        }
    }
}
