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

        if (EditorUtils.InPrefabEditor(target)) EditActorPrefabs(spawner);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditActorPrefabs(Spawner spawner)
    {
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
    }
}
