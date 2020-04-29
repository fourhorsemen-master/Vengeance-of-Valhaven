using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeMenuController : MonoBehaviour
{
    private Player player;

    private int numTreeVerticalSections;
    private Dictionary<Node, float> sectionIndices = new Dictionary<Node, float>();

    [SerializeField]
    private GameObject abilityTreeListContent = null;

    [SerializeField]
    private AbilityPanel abilityListingPanelPrefab = null;

    void Start()
    {
        player = RoomManager.Instance.Player;

        PopulateAbilityList();
        PopulateTree();
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

    private void PopulateTree()
    {
        numTreeVerticalSections = 0;
        sectionIndices.Clear();
        CalculateIndices(player.AbilityTree.RootNode);
    }

    private float CalculateIndices(Node node)
    {
        float Section;

        if (!node.HasChild(Direction.Left) && !node.HasChild(Direction.Right))
        {
            numTreeVerticalSections += 1;
            Section = numTreeVerticalSections - 0.5f;
        }
        else if (!node.HasChild(Direction.Right))
        {
            Section = CalculateIndices(node.GetChild(Direction.Left)) + 0.5f;
            numTreeVerticalSections += 1;
        }
        else if (!node.HasChild(Direction.Left))
        {
            numTreeVerticalSections += 1;
            Section = CalculateIndices(node.GetChild(Direction.Right)) - 0.5f;
        }
        else // Node has 2 children
        {
            float leftSection = CalculateIndices(node.GetChild(Direction.Left));
            float rightSection = CalculateIndices(node.GetChild(Direction.Right));
            Section = (leftSection + rightSection) / 2;
        }

        sectionIndices[node] = Section;
        return Section;
    }
}
