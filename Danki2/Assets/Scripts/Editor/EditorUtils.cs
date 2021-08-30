using System;
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
    /// <returns> True iff the button was pressed. </returns>
    public static bool IndentedButton(string label)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * LineHeight);

        bool pressed = GUILayout.Button(label);

        GUILayout.EndHorizontal();

        return pressed;
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
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target), typeof(T), false);
        EditorGUI.EndDisabledGroup();
    }
    
    /// <summary>
    /// Adds a field to edit a prefab.
    /// </summary>
    public static T PrefabField<T>(string label, T @object) where T : Object
    {
        return (T) EditorGUILayout.ObjectField(label, @object, typeof(T), false, null);
    }

    /// <summary>
    /// Edits all list items according to the input function and adds ability to edit the list size.
    /// </summary>
    public static void ResizeableList<T>(
        List<T> items,
        Func<T, T> editItem,
        T defaultValue,
        Action<T> itemAddedCallback = null,
        Action<T> itemRemovedCallback = null,
        int maxSize = -1
    )
    {
        ResizeableList(items, editItem, () => defaultValue, itemAddedCallback, itemRemovedCallback, maxSize);
    }

    /// <inheritdoc cref="ResizeableList{T}(System.Collections.Generic.List{T},System.Func{T,T},T,System.Action{T},System.Action{T})"/>
    public static void ResizeableList<T>(
        List<T> items,
        Func<T, T> editItem,
        Func<T> defaultValueProvider,
        Action<T> itemAddedCallback = null,
        Action<T> itemRemovedCallback = null,
        int maxSize = -1
    )
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i] = editItem(items[i]);
        }

        EditListSize(items, defaultValueProvider, itemAddedCallback, itemRemovedCallback, maxSize);
    }

    /// <inheritdoc cref="ResizeableList{T}(System.Collections.Generic.List{T},System.Func{T,T},T,System.Action{T},System.Action{T})"/>
    public static void ResizeableList<T>(
        List<T> items,
        Action<T> editItem,
        T defaultValue,
        Action<T> itemAddedCallback = null,
        Action<T> itemRemovedCallback = null,
        int maxSize = -1
    )
    {
        ResizeableList(items, editItem, () => defaultValue, itemAddedCallback, itemRemovedCallback, maxSize);
    }

    /// <inheritdoc cref="ResizeableList{T}(System.Collections.Generic.List{T},System.Func{T,T},T,System.Action{T},System.Action{T})"/>
    public static void ResizeableList<T>(
        List<T> items,
        Action<T> editItem,
        Func<T> defaultValueProvider,
        Action<T> itemAddedCallback = null,
        Action<T> itemRemovedCallback = null,
        int maxSize = -1
    )
    {
        items.ForEach(editItem);

        EditListSize(items, defaultValueProvider, itemAddedCallback, itemRemovedCallback, maxSize);
    }

    /// <summary>
    /// Adds buttons to add and remove elements from the given list. Items are always added to and removed
    /// from the end of the list.
    /// </summary>
    public static void EditListSize<T>(
        List<T> list,
        T defaultValue,
        Action<T> itemAddedCallback = null,
        Action<T> itemRemovedCallback = null,
        int maxSize = -1
    )
    {
        EditListSize(list, () => defaultValue, itemAddedCallback, itemRemovedCallback, maxSize);
    }

    /// <inheritdoc cref="EditListSize{T}(System.Collections.Generic.List{T},T,System.Action{T},System.Action{T})" />
    public static void EditListSize<T>(
        List<T> list,
        Func<T> defaultValueProvider,
        Action<T> itemAddedCallback = null,
        Action<T> itemRemovedCallback = null,
        int maxSize = -1
    )
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * LineHeight);

        EditorGUI.BeginDisabledGroup(list.Count == maxSize);
        if (GUILayout.Button("Add Item"))
        {
            T newItem = defaultValueProvider();
            list.Add(newItem);
            itemAddedCallback?.Invoke(newItem);
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(list.Count == 0);
        if (GUILayout.Button("Remove Item"))
        {
            int index = list.Count - 1;
            T oldItem = list[index];
            list.RemoveAt(index);
            itemRemovedCallback?.Invoke(oldItem);
        }
        EditorGUI.EndDisabledGroup();
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
