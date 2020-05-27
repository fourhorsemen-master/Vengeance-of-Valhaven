using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ChannelScreenBar))]
public class ChannelScreenBarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChannelScreenBar channelScreenBar = (ChannelScreenBar)target;

        foreach (ChannelType channelType in Enum.GetValues(typeof(ChannelType)))
        {
            channelScreenBar.barColourLookup[channelType] = EditorGUILayout.ColorField($"{channelType} Bar", channelScreenBar.barColourLookup[channelType]);
        }

        channelScreenBar.channelBar = EditorGUILayout.ObjectField("Bar Image", channelScreenBar.channelBar, typeof(Image), true) as Image;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
