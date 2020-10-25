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
            var cooldownDuringCombo = EditorGUILayout.Slider("CD during combo", player.Settings.CooldownDuringCombo, 0, 2);
            var cooldownAfterCombo = EditorGUILayout.Slider("CD after combo", player.Settings.CooldownAfterCombo, 0, 2);
            var comboTimeout = EditorGUILayout.Slider("Combo timeout", player.Settings.ComboTimeout, 2, 10);
            var rollResetsCombo = EditorGUILayout.Toggle("Roll resets combo", player.Settings.RollResetsCombo);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Roll", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            var totalRollCooldown = EditorGUILayout.Slider("Cooldown", player.Settings.TotalRollCooldown, 0, 5);
            var rollDuration = EditorGUILayout.Slider("Duration", player.Settings.RollDuration, 0, 1);
            var rollSpeedMultiplier = EditorGUILayout.Slider("Speed Multiplier", player.Settings.RollSpeedMultiplier, 1, 10);
        EditorGUI.indentLevel--;

        player.Settings = new PlayerSettings(
            cooldownDuringCombo,
            cooldownAfterCombo,
            comboTimeout,
            rollResetsCombo,
            totalRollCooldown,
            rollDuration,
            rollSpeedMultiplier
        );

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
