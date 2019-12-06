using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : MortalEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Player player = (Player)target;

        EditorGUILayout.LabelField("Dash", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        player.dashDuration = EditorGUILayout.Slider("Duration", player.dashDuration, 0, 1);
        player.dashSpeedMultiplier = EditorGUILayout.Slider("Speed Multiplier", player.dashSpeedMultiplier, 1, 10);
        player.totalDashCooldown = EditorGUILayout.Slider("Cooldown", player.totalDashCooldown, 0, 5);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Abilities", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        player.maximumCooldown = EditorGUILayout.Slider("Cooldown", player.maximumCooldown, 0, 5);
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
