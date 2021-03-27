using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollisionTemplateManager))]
public class CollisionTemplateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CollisionTemplateManager collisionTemplateManager = (CollisionTemplateManager)target;

        foreach (CollisionTemplateShape shape in Enum.GetValues(typeof(CollisionTemplateShape)))
        {
            collisionTemplateManager.prefabLookup[shape] = (CollisionTemplate)EditorGUILayout.ObjectField(
                shape.ToString(),
                collisionTemplateManager.prefabLookup[shape],
                typeof(CollisionTemplate),
                false,
                null
            );
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
