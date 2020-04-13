using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeMenuController : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject abilityTreeContent;

    [SerializeField]
    private GameObject abilityPanelPrefab;

    void Start()
    {
        PopulateAbilityList();
    }

    private void PopulateAbilityList()
    {
        //destroy all current panels
        foreach (Transform child in this.abilityTreeContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<AbilityReference, int> item in this.player.AbilityInventory)
        {
            if (item.Value > 0)
            {
                GameObject abilityPanel = Instantiate(this.abilityPanelPrefab, Vector3.zero, Quaternion.identity);
                abilityPanel.transform.parent = this.abilityTreeContent.transform;

                abilityPanel.GetComponent<AbilityPanel>().InitialisePanel(item.Key, item.Value);

                // determine Vector3 location for abilityPanel

                // link to icon, name and quantity - do on abilityPanel script where it can reference children
            }
        }
    }
}
