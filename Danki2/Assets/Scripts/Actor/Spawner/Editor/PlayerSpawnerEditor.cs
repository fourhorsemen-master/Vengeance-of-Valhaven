using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerSpawner))]
public class PlayerSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerSpawner spawner = (PlayerSpawner) target;

        EditorUtils.ShowScriptLink(spawner);

        spawner.Id = EditorGUILayout.IntField("ID", spawner.Id);

        if (EditorUtils.InPrefabEditor(target))
        {
            spawner.Spawner = (Spawner) EditorGUILayout.ObjectField("Spawner", spawner.Spawner, typeof(Spawner), true);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
