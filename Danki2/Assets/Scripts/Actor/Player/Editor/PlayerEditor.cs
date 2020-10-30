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
            player.cooldownDuringCombo = EditorGUILayout.Slider("CD during combo", player.cooldownDuringCombo, 0, 2);
            player.cooldownAfterCombo = EditorGUILayout.Slider("CD after combo", player.cooldownAfterCombo, 0, 2);
            player.comboTimeout = EditorGUILayout.Slider("Combo timeout", player.comboTimeout, 2, 10);
            player.feedbackTimeout = EditorGUILayout.Slider("Feedback timeout", player.comboTimeout, 0, 2);
            player.rollResetsCombo = EditorGUILayout.Toggle("Roll resets combo", player.rollResetsCombo);
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
