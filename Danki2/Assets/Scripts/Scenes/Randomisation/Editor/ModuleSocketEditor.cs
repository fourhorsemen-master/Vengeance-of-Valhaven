using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModuleSocket))]
public class ModuleSocketEditor : Editor
{
    private ModuleSocket moduleSocket;
    
    public override void OnInspectorGUI()
    {
        moduleSocket = (ModuleSocket) target;

        EditorUtils.ShowScriptLink(moduleSocket);

        if (EditorUtils.InPrefabEditor(target))
        {
            EditSocketType();
            EditNavBlocker();
            EditDirectionIndicator();
        }
        else
        {
            EditId();
            EditLockRotation();
            EditTags();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditSocketType()
    {
        moduleSocket.SocketType = (SocketType) EditorGUILayout.EnumPopup("Socket Type", moduleSocket.SocketType);
    }

    private void EditNavBlocker()
    {
        moduleSocket.NavBlocker = (GameObject) EditorGUILayout.ObjectField(
            "Nav Blocker",
            moduleSocket.NavBlocker,
            typeof(GameObject),
            true,
            null
        );
    }

    private void EditDirectionIndicator()
    {
        moduleSocket.DirectionIndicator = (GameObject) EditorGUILayout.ObjectField(
            "Direction Indicator",
            moduleSocket.DirectionIndicator,
            typeof(GameObject),
            true,
            null
        );
    }

    private void EditId()
    {
        moduleSocket.Id = EditorGUILayout.IntField("ID", moduleSocket.Id);
    }

    private void EditLockRotation()
    {
        moduleSocket.LockRotation = EditorGUILayout.Toggle("Lock Rotation", moduleSocket.LockRotation);
    }

    private void EditTags()
    {
        EditorUtils.Header("Tags");

        EditorGUI.indentLevel++;
        
        List<ModuleTag> tags = moduleSocket.Tags;
        
        for (int i = 0; i < tags.Count; i++)
        {
            tags[i] = (ModuleTag) EditorGUILayout.EnumPopup("Tag", tags[i]);
        }
        
        EditorUtils.VerticalSpace();

        EditorUtils.EditListSize("Add Tag", "Remove Tag", tags, ModuleTag.Short);

        EditorGUI.indentLevel--;
    }
}
