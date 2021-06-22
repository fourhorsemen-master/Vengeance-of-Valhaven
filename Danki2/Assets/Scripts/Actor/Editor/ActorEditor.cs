using UnityEngine;
using UnityEditor;

public class ActorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Actor actor = (Actor)target;

        EditorUtils.Header("Stats");

        EditorGUI.indentLevel++;

        EnumUtils.ForEach<Stat>(s =>
            actor.baseStats[s] = EditorGUILayout.IntField(s.ToString(), actor.baseStats[s])
        );

        EditorGUI.indentLevel--;

        if (GUI.changed) EditorUtility.SetDirty(target);
    }
}
