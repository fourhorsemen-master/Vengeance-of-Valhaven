using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : ActorEditor
{
    private AbilityTreeEditor _abilityTreeEditor = new AbilityTreeEditor();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Player player = (Player)target;

        _abilityTreeEditor.Edit(player);

        EditorGUILayout.LabelField("Abilities", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            player.abilityCooldown = EditorGUILayout.Slider("Cooldown", player.abilityCooldown, 0, 2);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Dash", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            player.totalDashCooldown = EditorGUILayout.Slider("Cooldown", player.totalDashCooldown, 0, 5);
            player.dashDuration = EditorGUILayout.Slider("Duration", player.dashDuration, 0, 1);
            player.dashSpeedMultiplier = EditorGUILayout.Slider("Speed Multiplier", player.dashSpeedMultiplier, 1, 10);
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
