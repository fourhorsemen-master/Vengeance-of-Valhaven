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
            EditTagsToExclude();
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
        GUILayout.BeginHorizontal();
        moduleSocket.Id = EditorGUILayout.IntField("ID", moduleSocket.Id);
        if (GUILayout.Button("Generate Random ID")) moduleSocket.Id = Random.Range(0, int.MaxValue);
        GUILayout.EndHorizontal();
    }

    private void EditLockRotation()
    {
        moduleSocket.LockRotation = EditorGUILayout.Toggle("Lock Rotation", moduleSocket.LockRotation);
    }

    private void EditTags()
    {
        EditorUtils.Header("Tags");

        EditorGUI.indentLevel++;
        
        EditorUtils.ResizeableList(
            moduleSocket.Tags,
            tag => (ModuleTag) EditorGUILayout.EnumPopup("Tag", tag),
            ModuleTag.Short
        );

        EditorGUI.indentLevel--;
    }

    private void EditTagsToExclude()
    {
        EditorUtils.Header("Tags To Exclude");

        EditorGUI.indentLevel++;
        
        EditorUtils.ResizeableList(
            moduleSocket.TagsToExclude,
            tag => (ModuleTag) EditorGUILayout.EnumPopup("Tag", tag),
            ModuleTag.Short
        );

        EditorGUI.indentLevel--;
    }
}
