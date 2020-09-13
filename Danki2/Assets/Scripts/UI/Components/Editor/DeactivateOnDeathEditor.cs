using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeactivateOnDeath))]
public class DeactivateOnDeathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DeactivateOnDeath deactivateOnDeath = (DeactivateOnDeath) target;

        deactivateOnDeath.usePlayer = EditorGUILayout.Toggle("Use player", deactivateOnDeath.usePlayer);
        
        if (!deactivateOnDeath.usePlayer)
        {
            deactivateOnDeath.actor = (Actor) EditorGUILayout.ObjectField(
                "Actor",
                deactivateOnDeath.actor,
                typeof(Actor),
                false
            );
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
