using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RarityLookup))]
public class RarityLookupEditor : Editor
{
    private EnumDictionary<Rarity, bool> augmentFoldoutStatus = new EnumDictionary<Rarity, bool>(false);

    public override void OnInspectorGUI()
    {
        RarityLookup rarityLookup = (RarityLookup) target;

        EditorUtils.ShowScriptLink(rarityLookup);

        EnumUtils.ForEach<Rarity>(rarity =>
        {
            EditorUtils.Header(rarity.ToString());
            EditorGUI.indentLevel++;
            EditRarityData(rarity, rarityLookup.Lookup[rarity]);
            EditorUtils.VerticalSpace();
            EditorGUI.indentLevel--;
        });
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditRarityData(Rarity rarity, RarityData rarityData)
    {
        rarityData.DisplayName = EditorGUILayout.TextField("Display Name", rarityData.DisplayName);
        rarityData.Weighting = EditorGUILayout.IntField("Weighting", rarityData.Weighting);
        rarityData.Colour = EditorGUILayout.ColorField("Colour", rarityData.Colour);
        rarityData.Frame = (Sprite) EditorGUILayout.ObjectField("Frame", rarityData.Frame, typeof(Sprite), false);

        augmentFoldoutStatus[rarity] = EditorGUILayout.Foldout(
            augmentFoldoutStatus[rarity],
            "Augmentations"
        );
        if (!augmentFoldoutStatus[rarity]) return;

        EditAugmentations(rarityData);
    }

    private void EditAugmentations(RarityData rarityData)
    {
        EditorGUI.indentLevel++;
        rarityData.Augmentations = rarityData.Augmentations ?? new List<Augmentation>();
        EditorUtils.ResizeableList(
            rarityData.Augmentations,
            EditAugmentation,
            new Augmentation(),
            itemName: "Augmentation"
        );
        EditorGUI.indentLevel--;
    }

    private Augmentation EditAugmentation(Augmentation augmentation)
    {
        augmentation.Descriptor = EditorGUILayout.TextField("Descriptor", augmentation.Descriptor);

        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            augmentation.Empowerments,
            x => (Empowerment)EditorGUILayout.EnumPopup("Empowerment", x),
            Empowerment.Impact,
            itemName: "Empowerment"
        );
        EditorGUI.indentLevel--;

        EditorUtils.VerticalSpace();

        return augmentation;
    }
}
