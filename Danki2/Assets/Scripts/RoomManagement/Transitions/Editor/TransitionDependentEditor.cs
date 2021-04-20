using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionDependent))]
public class TransitionDependentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TransitionDependent transitionDependentObject = (TransitionDependent) target;

        EditorUtils.ShowScriptLink(transitionDependentObject);

        EditType(transitionDependentObject);
        EditHasAssociatedEntrance(transitionDependentObject);
        EditAssociatedEntrance(transitionDependentObject);
        EditHasAssociatedExit(transitionDependentObject);
        EditAssociatedExit(transitionDependentObject);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditType(TransitionDependent transitionDependentObject)
    {
        transitionDependentObject.Type = (TransitionDependenceType) EditorGUILayout.EnumPopup(
            "Type",
            transitionDependentObject.Type
        );
    }
    
    private void EditHasAssociatedEntrance(TransitionDependent transitionDependentObject)
    {
        transitionDependentObject.HasAssociatedEntrance = EditorGUILayout.Toggle(
            "Has Associated Entrance",
            transitionDependentObject.HasAssociatedEntrance
        );
    }

    private void EditAssociatedEntrance(TransitionDependent transitionDependentObject)
    {
        EditorGUI.BeginDisabledGroup(!transitionDependentObject.HasAssociatedEntrance);
        transitionDependentObject.AssociatedEntranceId = EditorGUILayout.IntField(
            "Associated Entrance ID",
            transitionDependentObject.AssociatedEntranceId
        );
        EditorGUI.EndDisabledGroup();
    }

    private void EditHasAssociatedExit(TransitionDependent transitionDependentObject)
    {
        transitionDependentObject.HasAssociatedExit = EditorGUILayout.Toggle(
            "Has Associated Exit",
            transitionDependentObject.HasAssociatedExit
        );
    }

    private void EditAssociatedExit(TransitionDependent transitionDependentObject)
    {
        EditorGUI.BeginDisabledGroup(!transitionDependentObject.HasAssociatedExit);
        transitionDependentObject.AssociatedExitId = EditorGUILayout.IntField(
            "Associated Exit ID",
            transitionDependentObject.AssociatedExitId
        );
        EditorGUI.EndDisabledGroup();
    }
}
