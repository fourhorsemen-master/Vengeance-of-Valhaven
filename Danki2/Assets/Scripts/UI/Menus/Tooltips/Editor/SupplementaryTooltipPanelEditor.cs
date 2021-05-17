using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SupplementaryTooltipPanel))]
public class SupplementaryTooltipPanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SupplementaryTooltipPanel supplementaryTooltipPanel = (SupplementaryTooltipPanel)target;

        EditorUtils.ShowScriptLink(supplementaryTooltipPanel);

        if (EditorUtils.InPrefabEditor(target))
        {
            supplementaryTooltipPanel.displayDelay = EditorGUILayout.FloatField("Display Delay", supplementaryTooltipPanel.displayDelay);

            supplementaryTooltipPanel.rectTransform = (RectTransform)EditorGUILayout.ObjectField(
                "Rect Transform",
                supplementaryTooltipPanel.rectTransform,
                typeof(RectTransform),
                false
            );

            supplementaryTooltipPanel.supplementaryTooltipPrefab = EditorUtils.PrefabField(
                "Supplementary Tooltip Prefab",
                supplementaryTooltipPanel.supplementaryTooltipPrefab
            );
        }
        else
        {
            supplementaryTooltipPanel.tooltipRectTransform = (RectTransform)EditorGUILayout.ObjectField(
                "Tooltip Rect Transform",
                supplementaryTooltipPanel.tooltipRectTransform,
                typeof(RectTransform),
                false
            );
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
