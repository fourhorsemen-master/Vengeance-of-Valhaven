using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawnerEditor))]
public class EnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemySpawner spawner = (EnemySpawner) target;

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
