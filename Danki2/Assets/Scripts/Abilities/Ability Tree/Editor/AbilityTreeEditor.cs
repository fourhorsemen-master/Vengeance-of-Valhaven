using UnityEditor;
using UnityEngine;

public class AbilityTreeEditor
{
    public void Edit(Player player)
    {
        Debug.Log(player.AbilityTree.MaxDepth);

        Recurse(player.AbilityTree.GetChild(Direction.Left), "L");
        Recurse(player.AbilityTree.GetChild(Direction.Right), "R");

        if (GUILayout.Button("Reset"))
        {
            player.AbilityTree = AbilityTreeFactory.Default();
        }
    }

    public void Recurse(Node node, string label)
    {
        node.SetAbility((AbilityReference)EditorGUILayout.EnumPopup(label, node.GetAbility()));

        if (node.HasChild(Direction.Left))
        {
            Recurse(node.GetChild(Direction.Left), label + 'L');
        }

        if (node.HasChild(Direction.Right))
        {
            Recurse(node.GetChild(Direction.Right), label + 'R');
        }
    }
}
