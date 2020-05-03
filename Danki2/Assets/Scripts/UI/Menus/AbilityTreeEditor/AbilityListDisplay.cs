using System.Collections.Generic;
using UnityEngine;

public class AbilityListDisplay : MonoBehaviour
{
    private Player player;

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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<AbilityReference, int> item in player.AbilityInventory)
        {
            if (item.Value > 0)
            {
                AbilityPanel abilityPanel = Instantiate(abilityListingPanelPrefab, Vector3.zero, Quaternion.identity, transform);
                abilityPanel.Initialise(item.Key, item.Value);
            }
        }
    }
}
