using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuController))]
public class MenuControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MenuController menuController = (MenuController) target;

        EditorUtils.ShowScriptLink(menuController);
        
        EditMenuData(menuController.menuData);
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditMenuData(List<MenuData> menuData)
    {
        EditorUtils.Header("Menus");
        EditorGUI.indentLevel++;
        
        EditorUtils.ResizeableList(
            menuData,
            d =>
            {
                d.GameplayState = (GameplayState) EditorGUILayout.EnumPopup("Gameplay State", d.GameplayState);
                d.Menu = (GameObject) EditorGUILayout.ObjectField("Menu", d.Menu, typeof(GameObject), true);
                EditorUtils.VerticalSpace();
            },
            () => new MenuData()
        );
        
        EditorGUI.indentLevel--;
    }
}
