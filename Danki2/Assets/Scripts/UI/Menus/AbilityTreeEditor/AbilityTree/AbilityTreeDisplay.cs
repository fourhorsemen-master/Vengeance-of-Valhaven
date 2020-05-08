using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class AbilityTreeDisplay : MonoBehaviour
{
    [SerializeField]
    private RectTransform containingPanel = null;

    [SerializeField]
    private RectTransform treeRowsPanel = null;

    [SerializeField]
    private RectTransform treeRowPanel = null;

    [SerializeField]
    private TreeAbility treeAbilityPrefab = null;

    [SerializeField]
    private Sprite rootNodeSprite = null;

    private Player player;
    private float numTreeVerticalSections;
    private int maxTreeDepth;
    private Dictionary<Node, float> sectionIndices = new Dictionary<Node, float>();
    private bool nodesDrawn = false;
    public void Start()
    {
        player = RoomManager.Instance.Player;
        RecalculateDisplay();
    }

    private void RecalculateDisplay()
    {
        numTreeVerticalSections = 0;
        sectionIndices.Clear();
        CalculateIndices(player.AbilityTree.RootNode);

        for (int i = 0; i < treeRowsPanel.childCount; i++)
        {
            Destroy(treeRowsPanel.GetChild(i));
        }

        for (int i = 0; i < maxTreeDepth + 1; i++)
        {
            Instantiate(treeRowPanel, treeRowsPanel.transform, false);
        }

        nodesDrawn = false;
    }

    private void Update()
    {
        if (!nodesDrawn)
        {
            DrawNodes(player.AbilityTree.RootNode);
            nodesDrawn = true;
        }
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
            numTreeVerticalSections += 0.25f;
        }
        else if (!node.HasChild(Direction.Left))
        {
            numTreeVerticalSections += 0.25f;
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

    private TreeAbility DrawNodes(Node node, int row = 0)
    {
        float panelWidth = containingPanel.rect.width;

        Transform treeRow = treeRowsPanel.GetChild(row);

        TreeAbility treeAbility = Instantiate(treeAbilityPrefab, treeRow.transform, false);
        treeAbility.ShiftRight(panelWidth * (sectionIndices[node] / numTreeVerticalSections));

        if (node.IsRootNode())
        {
            treeAbility.SetImage(rootNodeSprite);
            treeAbility.RemoveOverlay();
        }
        else
        {
            treeAbility.SetImage(AbilityIconManager.Instance.GetIcon(node.Ability));
        }

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
}