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

        if (EditorUtils.InPrefabEditor(target)) EditSocketType();
        else
        {
            EditId();
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

    private void EditId()
    {
        moduleSocket.Id = EditorGUILayout.IntField("ID", moduleSocket.Id);
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
