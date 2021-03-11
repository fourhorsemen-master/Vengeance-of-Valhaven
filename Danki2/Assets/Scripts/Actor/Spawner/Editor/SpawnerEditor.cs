using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Spawner spawner = (Spawner) target;

        EditorUtils.ShowScriptLink(spawner);

        spawner.id = EditorGUILayout.IntField("ID", spawner.id);

        EditorUtils.Header("Actor prefabs");
        EditorGUI.indentLevel++;

        EnumUtils.ForEach<ActorType>(actorType =>
        {
            spawner.prefabLookup[actorType] = (Actor) EditorGUILayout.ObjectField(
                actorType.ToString(),
                spawner.prefabLookup[actorType],
                typeof(Actor),
                false,
                null
            );
        });

        EditorGUI.indentLevel++;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
