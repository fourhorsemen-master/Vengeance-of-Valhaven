using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollisionTemplateManager))]
public class CollisionTemplateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CollisionTemplateManager collisionTemplateManager = (CollisionTemplateManager)target;

        foreach (CollisionTemplate template in Enum.GetValues(typeof(CollisionTemplate)))
        {
            collisionTemplateManager.prefabLookup[template] = (MeshCollider)EditorGUILayout.ObjectField(
                template.ToString(),
                collisionTemplateManager.prefabLookup[template],
                typeof(MeshCollider),
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
