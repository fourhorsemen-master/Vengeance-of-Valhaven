using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTreeMenuController : MonoBehaviour
{
    private Player player;

    private float numTreeVerticalSections;
    private Dictionary<Node, float> sectionIndices = new Dictionary<Node, float>();

    [SerializeField]
    private GameObject abilityTreeListContent = null;

    [SerializeField]
    private AbilityPanel abilityListingPanelPrefab = null;

    [SerializeField]
    private Image rootNodeOrb = null;

    [SerializeField]
    private RectTransform treeRowsPanel = null;

    [SerializeField]
    private RectTransform treeRowPanel = null;

    [SerializeField]
    private Image treeAbilityImage = null;
    private int maxTreeDepth;

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
        DrawNodes(player.AbilityTree.RootNode);
    }

    private float CalculateIndices(Node node, int depth = 0)
    {
        float Section;

        if (!node.HasChild(Direction.Left) && !node.HasChild(Direction.Right))
        {
            numTreeVerticalSections += 1;
            Section = numTreeVerticalSections - 0.5f;
        }
        else if (!node.HasChild(Direction.Right))
        {
            Section = CalculateIndices(node.GetChild(Direction.Left), depth + 1) + 0.5f;
            numTreeVerticalSections += 0.5f;
        }
        else if (!node.HasChild(Direction.Left))
        {
            numTreeVerticalSections += 0.5f;
            Section = CalculateIndices(node.GetChild(Direction.Right), depth + 1) - 0.5f;
        }
        else // Node has 2 children
        {
            float leftSection = CalculateIndices(node.GetChild(Direction.Left), depth + 1);
            float rightSection = CalculateIndices(node.GetChild(Direction.Right), depth + 1);
            Section = (leftSection + rightSection) / 2;
        }

        maxTreeDepth = Math.Max(maxTreeDepth, depth);
        sectionIndices[node] = Section;
        return Section;
    }

    private void DrawNodes(Node node, int row = 0)
    {
        float panelWidth = rootNodeOrb.transform.parent.GetComponent<RectTransform>().rect.width;

        if (row == 0)
        {
            Vector3 newPosition = rootNodeOrb.rectTransform.localPosition;
            newPosition += Vector3.right * panelWidth * (sectionIndices[node] / numTreeVerticalSections);

            rootNodeOrb.rectTransform.localPosition = newPosition;

            for (int i = 0; i < treeRowsPanel.childCount; i++)
            {
                Destroy(treeRowsPanel.GetChild(i));
            }

            for (int i = 0; i < maxTreeDepth; i++)
            {
                Instantiate(treeRowPanel, treeRowsPanel.transform, false);
            }
        }
        else
        {
            Transform treeRow = treeRowsPanel.GetChild(row - 1);

            Image treeAbility = Instantiate(treeAbilityImage, treeRow.transform, false);

            Vector3 newPosition = treeAbility.rectTransform.localPosition;
            newPosition += Vector3.right * panelWidth * (sectionIndices[node] / numTreeVerticalSections);

            treeAbility.rectTransform.localPosition = newPosition;

            treeAbility.sprite = AbilityIconManager.Instance.GetIcon(node.Ability);
        }

        if (node.HasChild(Direction.Left))
        {
            DrawNodes(node.GetChild(Direction.Left), row + 1);
        }

        if (node.HasChild(Direction.Right))
        {
            DrawNodes(node.GetChild(Direction.Right), row + 1);
        }
    }
}
