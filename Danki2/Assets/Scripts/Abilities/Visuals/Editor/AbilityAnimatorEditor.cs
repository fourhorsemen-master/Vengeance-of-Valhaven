using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityAnimator))]
public class AbilityAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AbilityAnimator abilityAnimator = (AbilityAnimator) target;
        
        EditorUtils.Header("Animation Types");
        EditorGUI.indentLevel++;
        EnumUtils.ForEach<AbilityType>(abilityType =>
        {
            abilityAnimator.abilityTypeToAnimationTypeDictionary[abilityType] = (AbilityAnimationType) EditorGUILayout.EnumPopup(
                abilityType.ToString(),
                abilityAnimator.abilityTypeToAnimationTypeDictionary[abilityType]
            );

            abilityAnimator.abilityTypeToAnimationSpeedCurveDictionary[abilityType] = EditorGUILayout.CurveField(
                abilityAnimator.abilityTypeToAnimationSpeedCurveDictionary[abilityType]
            );
        });
        EditorGUI.indentLevel--;
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
