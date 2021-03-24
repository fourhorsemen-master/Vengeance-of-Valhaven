using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConditionalBlocker))]
public class ConditionalBlockerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ConditionalBlocker conditionalBlocker = (ConditionalBlocker) target;

        EditorUtils.ShowScriptLink(conditionalBlocker);

        EditHasAssociatedEntrance(conditionalBlocker);
        EditAssociatedEntrance(conditionalBlocker);
        EditHasAssociatedExit(conditionalBlocker);
        EditAssociatedExit(conditionalBlocker);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditHasAssociatedEntrance(ConditionalBlocker conditionalBlocker)
    {
        conditionalBlocker.HasAssociatedEntrance = EditorGUILayout.Toggle(
            "Has Associated Entrance",
            conditionalBlocker.HasAssociatedEntrance
        );
    }

    private void EditAssociatedEntrance(ConditionalBlocker conditionalBlocker)
    {
        EditorGUI.BeginDisabledGroup(!conditionalBlocker.HasAssociatedEntrance);
        conditionalBlocker.AssociatedEntranceId = EditorGUILayout.IntField(
            "Associated Entrance ID",
            conditionalBlocker.AssociatedEntranceId
        );
        EditorGUI.EndDisabledGroup();
    }

    private void EditHasAssociatedExit(ConditionalBlocker conditionalBlocker)
    {
        conditionalBlocker.HasAssociatedExit = EditorGUILayout.Toggle(
            "Has Associated Exit",
            conditionalBlocker.HasAssociatedExit
        );
    }

    private void EditAssociatedExit(ConditionalBlocker conditionalBlocker)
    {
        EditorGUI.BeginDisabledGroup(!conditionalBlocker.HasAssociatedExit);
        conditionalBlocker.AssociatedExitId = EditorGUILayout.IntField(
            "Associated Exit ID",
            conditionalBlocker.AssociatedExitId
        );
        EditorGUI.EndDisabledGroup();
    }
}
