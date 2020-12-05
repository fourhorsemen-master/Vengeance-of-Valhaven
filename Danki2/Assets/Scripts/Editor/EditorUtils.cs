﻿using System;
using UnityEditor;
using UnityEngine;

public static class EditorUtils
{
    private const int LineHeight = 15;

    private static readonly GUIStyle MultilineTextFieldStyle = new GUIStyle(EditorStyles.textField) {wordWrap = true};
    private static readonly GUIStyle MultilineLabelFieldStyle = new GUIStyle(EditorStyles.label) {wordWrap = true};

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
}
