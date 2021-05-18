using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DevMapGenerationLookup))]
public class DevMapGenerationLookupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DevMapGenerationLookup devMapGenerationLookup = (DevMapGenerationLookup) target;

        EditorUtils.ShowScriptLink(devMapGenerationLookup);

        int roomsPerZone = devMapGenerationLookup.RoomsPerZoneLookup.Values.First();
        roomsPerZone = EditorGUILayout.IntField("Rooms Per Zone", roomsPerZone);
        EnumUtils.ForEach<Zone>(z => devMapGenerationLookup.RoomsPerZoneLookup[z] = roomsPerZone);
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
