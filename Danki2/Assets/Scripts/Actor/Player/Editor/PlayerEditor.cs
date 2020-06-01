using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : ActorEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Player player = (Player)target;

        EditorGUILayout.LabelField("Abilities", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            player.abilityCooldown = EditorGUILayout.Slider("Cooldown", player.abilityCooldown, 0, 2);
            player.abilityTimeoutLimit = EditorGUILayout.Slider("Timeout", player.abilityTimeoutLimit, 2, 10);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Roll", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            player.totalRollCooldown = EditorGUILayout.Slider("Cooldown", player.totalRollCooldown, 0, 5);
            player.rollDuration = EditorGUILayout.Slider("Duration", player.rollDuration, 0, 1);
            player.rollSpeedMultiplier = EditorGUILayout.Slider("Speed Multiplier", player.rollSpeedMultiplier, 1, 10);
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
