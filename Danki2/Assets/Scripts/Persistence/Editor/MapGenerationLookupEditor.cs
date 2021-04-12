using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerationLookup))]
public class MapGenerationLookupEditor : Editor
{
    private MapGenerationLookup mapGenerationLookup;
    
    public override void OnInspectorGUI()
    {
        mapGenerationLookup = (MapGenerationLookup) target;

        EditorUtils.ShowScriptLink(mapGenerationLookup);
        
        EditAbilityChoices();
        EditDepthData();
        EditExitData();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditAbilityChoices()
    {
        mapGenerationLookup.AbilityChoices = EditorGUILayout.IntField("Ability Choices", mapGenerationLookup.AbilityChoices);
    }
    
    private void EditDepthData()
    {
        mapGenerationLookup.MinRoomDepth = EditorGUILayout.IntField("Min Room Depth", mapGenerationLookup.MinRoomDepth);
        mapGenerationLookup.MaxRoomDepth = EditorGUILayout.IntField("Max Room Depth", mapGenerationLookup.MaxRoomDepth);

    }

    private void EditExitData()
    {
        mapGenerationLookup.MinRoomExits = EditorGUILayout.IntField("Min Room Exits", mapGenerationLookup.MinRoomExits);
        mapGenerationLookup.MaxRoomExits = EditorGUILayout.IntField("Max Room Exits", mapGenerationLookup.MaxRoomExits);
    }
}
