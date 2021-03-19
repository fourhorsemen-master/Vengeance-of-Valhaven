using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionSocket))]
public class TransitionSocketEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TransitionSocket transitionSocket = (TransitionSocket) target;
        
        EditorUtils.ShowScriptLink(transitionSocket);

        if (EditorUtils.InPrefabEditor(target))
        {
            EditNavBlocker(transitionSocket);
            EditDirectionIndicator(transitionSocket);
        }
        else
        {
            EditId(transitionSocket);
            EditHasAssociatedEntrance(transitionSocket);
            EditAssociatedEntrance(transitionSocket);
            EditHasAssociatedExit(transitionSocket);
            EditAssociatedExit(transitionSocket);
            EditTags(transitionSocket.Tags);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditNavBlocker(TransitionSocket transitionSocket)
    {
        transitionSocket.NavBlocker = (GameObject) EditorGUILayout.ObjectField(
            "Nav Blocker",
            transitionSocket.NavBlocker,
            typeof(GameObject),
            true,
            null
        );
    }

    private void EditDirectionIndicator(TransitionSocket transitionSocket)
    {
        transitionSocket.DirectionIndicator = (GameObject) EditorGUILayout.ObjectField(
            "Direction Indicator",
            transitionSocket.DirectionIndicator,
            typeof(GameObject),
            true,
            null
        );
    }
    
    private void EditId(TransitionSocket transitionSocket)
    {
        transitionSocket.Id = EditorGUILayout.IntField("ID", transitionSocket.Id);
    }
    
    private void EditHasAssociatedEntrance(TransitionSocket transitionSocket)
    {
        transitionSocket.HasAssociatedEntrance = EditorGUILayout.Toggle(
            "Has Associated Entrance",
            transitionSocket.HasAssociatedEntrance
        );
    }

    private void EditAssociatedEntrance(TransitionSocket transitionSocket)
    {
        EditorGUI.BeginDisabledGroup(!transitionSocket.HasAssociatedEntrance);
        transitionSocket.AssociatedEntranceId = EditorGUILayout.IntField(
            "Associated Entrance ID",
            transitionSocket.AssociatedEntranceId
        );
        EditorGUI.EndDisabledGroup();
    }

    private void EditHasAssociatedExit(TransitionSocket transitionSocket)
    {
        transitionSocket.HasAssociatedExit = EditorGUILayout.Toggle(
            "Has Associated Exit",
            transitionSocket.HasAssociatedExit
        );
    }

    private void EditAssociatedExit(TransitionSocket transitionSocket)
    {
        EditorGUI.BeginDisabledGroup(!transitionSocket.HasAssociatedExit);
        transitionSocket.AssociatedExitId = EditorGUILayout.IntField(
            "Associated Exit ID",
            transitionSocket.AssociatedExitId
        );
        EditorGUI.EndDisabledGroup();
    }
    
    private void EditTags(List<ModuleTag> tags)
    {
        EditorUtils.Header("Tags");

        EditorGUI.indentLevel++;

        for (int i = 0; i < tags.Count; i++)
        {
            tags[i] = (ModuleTag) EditorGUILayout.EnumPopup("Tag", tags[i]);
        }

        EditorUtils.EditListSize("Add Tag", "Remove Tag", tags, ModuleTag.Short);

        EditorGUI.indentLevel--;
    }
}
