﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class EditorUtils
{
    private const int LineHeight = 15;

    private static readonly GUIStyle MultilineTextFieldStyle = new GUIStyle(EditorStyles.textField) {wordWrap = true};
    private static readonly GUIStyle MultilineLabelFieldStyle = new GUIStyle(EditorStyles.label) {wordWrap = true};

    private static readonly float DefaultVerticalSpace = 8;

    /// <summary>
    /// Adds a text field that wraps text and has multiple lines.
    /// </summary>
    /// <param name="label"> The text field label. </param>
    /// <param name="text"> The text to edit. </param>
    /// <param name="lineCount"> The number of lines for the text field to have. </param>
    /// <returns> The text entered by the user. </returns>
    public static string MultilineTextField(string label, string text, int lineCount)
    {
        return EditorGUILayout.TextField(label, text, MultilineTextFieldStyle, GUILayout.Height(lineCount * LineHeight));
    }

    /// <summary>
    /// Label fields don't wrap onto multiple lines by default. Helper method for label field that will wrap.
    /// </summary>
    /// <param name="label"> The label. </param>
    public static void MultilineLabelField(string label)
    {
        EditorGUILayout.LabelField(label, MultilineLabelFieldStyle);
    }

    /// <summary>
    /// Adds button that is indented with the current indent level.
    /// </summary>
    /// <param name="label"> The button label. </param>
    /// <param name="action"> The action to run if the button is pressed. </param>
    public static void IndentedButton(string label, Action action)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * LineHeight);

        if (GUILayout.Button(label)) action();

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Adds a header with the same styling as the Header attribute.
    /// </summary>
    /// <param name="label"> The header label. </param>
    public static void Header(string label)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
    }

    /// <summary>
    /// Displays a link to the script in the same way as the Unity default editor.
    /// </summary>
    public static void ShowScriptLink<T>(T target) where T : MonoBehaviour
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target), typeof(T), false);
        GUI.enabled = true;
    }
    
    /// <summary>
    /// Adds a field to edit a prefab.
    /// </summary>
    public static T PrefabField<T>(string label, T @object) where T : Object
    {
        return (T) EditorGUILayout.ObjectField(label, @object, typeof(T), false, null);
    }

    /// <summary>
    /// Adds buttons to add and remove elements from the given list.
    /// </summary>
    public static void EditListSize<T>(string addLabel, string removeLabel, List<T> list, T defaultValue)
    {
        EditListSize(addLabel, removeLabel, list, () => defaultValue);
    }

    /// <inheritdoc cref="EditListSize{T}(string,string,System.Collections.Generic.List{T},T)" />
    public static void EditListSize<T>(string addLabel, string removeLabel, List<T> list, Func<T> defaultValueProvider)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * LineHeight);

        if (GUILayout.Button(addLabel)) list.Add(defaultValueProvider());

        GUI.enabled = list.Count > 0;
        if (GUILayout.Button(removeLabel)) list.RemoveAt(list.Count - 1);
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Adds a small amount of vertical space.
    /// </summary>
    public static void VerticalSpace()
    {
        GUILayout.Space(DefaultVerticalSpace);
    }

    /// <summary>
    /// Returns true if the target is being edited in the prefab editor, rather than editing an instance
    /// of a prefab.
    /// </summary>
    public static bool InPrefabEditor(Object target)
    {
        return PrefabUtility.GetPrefabInstanceStatus(target) == PrefabInstanceStatus.NotAPrefab;
    }
}
