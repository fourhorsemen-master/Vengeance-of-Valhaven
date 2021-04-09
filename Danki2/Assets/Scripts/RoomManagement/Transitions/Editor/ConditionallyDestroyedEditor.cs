using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConditionallyDestroyed))]
public class ConditionallyDestroyedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ConditionallyDestroyed conditionallyDestroyedObject = (ConditionallyDestroyed) target;

        EditorUtils.ShowScriptLink(conditionallyDestroyedObject);

        EditHasAssociatedEntrance(conditionallyDestroyedObject);
        EditAssociatedEntrance(conditionallyDestroyedObject);
        EditHasAssociatedExit(conditionallyDestroyedObject);
        EditAssociatedExit(conditionallyDestroyedObject);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditHasAssociatedEntrance(ConditionallyDestroyed conditionallyDestroyedObject)
    {
        conditionallyDestroyedObject.HasAssociatedEntrance = EditorGUILayout.Toggle(
            "Has Associated Entrance",
            conditionallyDestroyedObject.HasAssociatedEntrance
        );
    }

    private void EditAssociatedEntrance(ConditionallyDestroyed conditionallyDestroyedObject)
    {
        EditorGUI.BeginDisabledGroup(!conditionallyDestroyedObject.HasAssociatedEntrance);
        conditionallyDestroyedObject.AssociatedEntranceId = EditorGUILayout.IntField(
            "Associated Entrance ID",
            conditionallyDestroyedObject.AssociatedEntranceId
        );
        EditorGUI.EndDisabledGroup();
    }

    private void EditHasAssociatedExit(ConditionallyDestroyed conditionallyDestroyedObject)
    {
        conditionallyDestroyedObject.HasAssociatedExit = EditorGUILayout.Toggle(
            "Has Associated Exit",
            conditionallyDestroyedObject.HasAssociatedExit
        );
    }

    private void EditAssociatedExit(ConditionallyDestroyed conditionallyDestroyedObject)
    {
        EditorGUI.BeginDisabledGroup(!conditionallyDestroyedObject.HasAssociatedExit);
        conditionallyDestroyedObject.AssociatedExitId = EditorGUILayout.IntField(
            "Associated Exit ID",
            conditionallyDestroyedObject.AssociatedExitId
        );
        EditorGUI.EndDisabledGroup();
    }
}
