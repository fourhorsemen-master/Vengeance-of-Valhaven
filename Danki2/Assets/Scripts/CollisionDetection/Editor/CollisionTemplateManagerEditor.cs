using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollisionTemplateManager))]
public class CollisionTemplateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CollisionTemplateManager collisionTemplateManager = (CollisionTemplateManager)target;

        foreach (CollisionTemplateShape template in Enum.GetValues(typeof(CollisionTemplateShape)))
        {
            collisionTemplateManager.prefabLookup[template] = (CollisionTemplate)EditorGUILayout.ObjectField(
                template.ToString(),
                collisionTemplateManager.prefabLookup[template],
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
