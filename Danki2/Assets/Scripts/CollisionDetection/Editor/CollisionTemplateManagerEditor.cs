using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollisionTemplateManager))]
public class CollisionTemplateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CollisionTemplateManager collisionTemplateManager = (CollisionTemplateManager)target;

        EnumUtils.ForEach<CollisionTemplateShape>(shape =>
            EditorUtils.PrefabField(shape.ToString(), collisionTemplateManager.prefabLookup[shape])
        );

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
