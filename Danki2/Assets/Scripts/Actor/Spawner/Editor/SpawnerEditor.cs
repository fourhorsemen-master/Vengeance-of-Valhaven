using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Spawner spawner = (Spawner) target;

        spawner.id = EditorGUILayout.IntField("ID", spawner.id);

        EnumUtils.ForEach<ActorType>(actorType =>
        {
            spawner.prefabLookup[actorType] = (Actor) EditorGUILayout.ObjectField(
                $"{actorType.ToString()} prefab",
                spawner.prefabLookup[actorType],
                typeof(Actor),
                false,
                null
            );
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
