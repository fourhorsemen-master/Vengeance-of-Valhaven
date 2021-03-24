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
            EditAssociatedExitId(transitionSocket);
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

    private void EditAssociatedExitId(TransitionSocket transitionSocket)
    {
        transitionSocket.AssociatedExitId = EditorGUILayout.IntField(
            "Associated Exit ID",
            transitionSocket.AssociatedExitId
        );
    }
}
