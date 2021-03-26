using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RarityLookup))]
public class RarityLookupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RarityLookup rarityLookup = (RarityLookup) target;

        EditorUtils.ShowScriptLink(rarityLookup);

        EnumUtils.ForEach<Rarity>(rarity =>
        {
            EditorUtils.Header(rarity.ToString());
            EditorGUI.indentLevel++;
            EditRarityData(rarityLookup.Lookup[rarity]);
            EditorGUI.indentLevel--;
        });
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditRarityData(RarityData rarityData)
    {
        rarityData.DisplayName = EditorGUILayout.TextField("Display Name", rarityData.DisplayName);
        rarityData.Colour = EditorGUILayout.ColorField("Colour", rarityData.Colour);
        rarityData.Frame = (Sprite) EditorGUILayout.ObjectField("Frame", rarityData.Frame, typeof(Sprite), false);
    }
}
