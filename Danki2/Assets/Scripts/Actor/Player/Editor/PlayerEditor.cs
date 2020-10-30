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
            player.CooldownDuringCombo = EditorGUILayout.Slider("CD during combo", player.CooldownDuringCombo, 0, 2);
            player.CooldownAfterCombo = EditorGUILayout.Slider("CD after combo", player.CooldownAfterCombo, 0, 2);
            player.ComboTimeout = EditorGUILayout.Slider("Combo timeout", player.ComboTimeout, 2, 10);
            player.FeedbackTimeout = EditorGUILayout.Slider("Feedback timeout", player.ComboTimeout, 0, 2);
            player.RollResetsCombo = EditorGUILayout.Toggle("Roll resets combo", player.RollResetsCombo);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Roll", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            player.TotalRollCooldown = EditorGUILayout.Slider("Cooldown", player.TotalRollCooldown, 0, 5);
            player.RollDuration = EditorGUILayout.Slider("Duration", player.RollDuration, 0, 1);
            player.RollSpeedMultiplier = EditorGUILayout.Slider("Speed Multiplier", player.RollSpeedMultiplier, 1, 10);
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
