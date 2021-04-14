using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraVolume))]
public class CameraVolumeEditor : Editor
{
    private CameraVolume cameraVolume;

    public override void OnInspectorGUI()
    {
        cameraVolume = (CameraVolume) target;

        EditorUtils.ShowScriptLink(cameraVolume);

        if (EditorUtils.InPrefabEditor(target))
        {
            EditMeshRenderer();
            EditCameraTransformLookup();
            EditPoleColorLookup();
        }
        else
        {
            EditSmoothFactor();
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditMeshRenderer()
    {
        cameraVolume.MeshRenderer = (MeshRenderer) EditorGUILayout.ObjectField(
            "Mesh Renderer",
            cameraVolume.MeshRenderer,
            typeof(MeshRenderer),
            true
        );
    }

    private void EditCameraTransformLookup()
    {
        EditorUtils.Header("Camera Transforms");
        EditorGUI.indentLevel++;
        
        EnumUtils.ForEach<Pole>(pole =>
        {
            cameraVolume.CameraTransformLookup[pole] = (Transform) EditorGUILayout.ObjectField(
                pole.ToString(),
                cameraVolume.CameraTransformLookup[pole],
                typeof(Transform),
                true
            );
        });
        
        EditorGUI.indentLevel--;
    }

    private void EditPoleColorLookup()
    {
        EditorUtils.Header("Camera Colors");
        EditorGUI.indentLevel++;
        
        EnumUtils.ForEach<Pole>(pole =>
        {
            cameraVolume.PoleColorLookup[pole] = EditorGUILayout.ColorField(
                pole.ToString(),
                cameraVolume.PoleColorLookup[pole]
            );
        });
        
        EditorGUI.indentLevel--;
    }

    private void EditSmoothFactor()
    {
        cameraVolume.OverrideSmoothFactor = EditorGUILayout.Toggle(
            "Override Smooth Factor",
            cameraVolume.OverrideSmoothFactor
        );
        if (!cameraVolume.OverrideSmoothFactor) EditorGUI.BeginDisabledGroup(true);
        cameraVolume.SmoothFactorOverride = EditorGUILayout.FloatField(
            "Smooth Factor Override",
            cameraVolume.SmoothFactorOverride
        );
        if (!cameraVolume.OverrideSmoothFactor) EditorGUI.EndDisabledGroup();
    }
}
