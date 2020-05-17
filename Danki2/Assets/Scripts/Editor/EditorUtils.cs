using UnityEditor;
using UnityEngine;

public static class EditorUtils
{
    private const int LineHeight = 15;

    private static readonly GUIStyle MultilineTextFieldStyle = new GUIStyle(EditorStyles.textField) {wordWrap = true};

    public static string MultilineTextField(string label, string text, int lineCount)
    {
        return EditorGUILayout.TextField(label, text, MultilineTextFieldStyle, GUILayout.Height(lineCount * LineHeight));
    }
}
