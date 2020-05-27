using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeDisplay : MonoBehaviour
{
    [SerializeField]
    private RectTransform containingPanel = null;

    [SerializeField]
    private RectTransform treeRowsPanel = null;

    [SerializeField]
    private RectTransform treeRowPanelPrefab = null;

    [SerializeField]
    private TreeAbility treeAbilityPrefab = null;

    private AbilityTree abilityTree;
    private float numTreeVerticalSections;
    private Dictionary<Node, float> sectionIndices = new Dictionary<Node, float>();

    private void OnEnable()
    {
        Player player = RoomManager.Instance.Player;
        abilityTree = player.AbilityTree;

        if (abilityTree != null) RecalculateDisplay();
        // TODO: subscribe to changes in the Ability Tree to recalculate the display.
    }

    /// <summary>
    /// Clear any previously calculated sections data and remove all rows of the ability tree display and re-add rows according to the depth of the ability tree.
    /// Also set nodes to be redrawn next frame.
    /// </summary>
    public void RecalculateDisplay()
    {
        numTreeVerticalSections = 0;
        sectionIndices.Clear();
        CalculateIndices(abilityTree.RootNode);

        for (int i = 0; i < treeRowsPanel.childCount; i++)
        {
            Destroy(treeRowsPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < abilityTree.MaxDepth + 1; i++)
        {
            Instantiate(treeRowPanelPrefab, treeRowsPanel.transform, false);
        }

        this.WaitAndAct(0f, () => DrawNodes(abilityTree.RootNode));
    }

    /// <summary>
    /// Bottom up, left to right recursive algorithm that calculates horizontal positions for all tree nodes where:
    /// - Each leaf node sits in it's own vertical 'section'
    /// - Parents of two bisect their childrens' horizontal positions
    /// - parents of one are shifted half a section away from their child
    /// </summary>
    /// <param name="node"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private float CalculateIndices(Node node, int depth = 0)
    {
        float section;

        if (!node.HasChild(Direction.Left) && !node.HasChild(Direction.Right))
        {
            numTreeVerticalSections += 1;
            section = numTreeVerticalSections - 0.5f;
        }
        else if (!node.HasChild(Direction.Right))
        {
            section = CalculateIndices(node.GetChild(Direction.Left), depth + 1) + 0.5f;
            numTreeVerticalSections += 0.25f;
        }
        else if (!node.HasChild(Direction.Left))
        {
            numTreeVerticalSections += 0.25f;
            section = CalculateIndices(node.GetChild(Direction.Right), depth + 1) - 0.5f;
        }
        else // Node has 2 children
        {
            float leftSection = CalculateIndices(node.GetChild(Direction.Left), depth + 1);
            float rightSection = CalculateIndices(node.GetChild(Direction.Right), depth + 1);
            section = (leftSection + rightSection) / 2;
        }

        sectionIndices[node] = section;
        return section;
    }

    /// <summary>
    /// Top down, left to right recursive method that draws each node based on positions calculated in CalculateIndices and connects parents to their children.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    private TreeAbility DrawNodes(Node node, int row = 0)
    {

        Transform treeRow = treeRowsPanel.GetChild(row);
        TreeAbility treeAbility = AddTreeAbility(treeRow, node);

        if (node.HasChild(Direction.Left))
        {
            treeAbility.ConnectToChild(
                DrawNodes(node.GetChild(Direction.Left), row + 1),
                Direction.Left
            );
        }

        if (node.HasChild(Direction.Right))
        {
            treeAbility.ConnectToChild(
                DrawNodes(node.GetChild(Direction.Right), row + 1),
                Direction.Right
            );
        }

        return treeAbility;
    }

    private TreeAbility AddTreeAbility(Transform treeRow, Node node)
    {
        TreeAbility treeAbility = Instantiate(treeAbilityPrefab, treeRow.transform, false);

        float panelWidth = containingPanel.rect.width;
        treeAbility.ShiftRight(panelWidth * (sectionIndices[node] / numTreeVerticalSections));

        treeAbility.SetNode(node);

        return treeAbility;
    }
}