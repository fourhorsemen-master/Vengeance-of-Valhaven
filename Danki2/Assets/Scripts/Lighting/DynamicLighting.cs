using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[Serializable]
public class DynamicLightingData
{
    [SerializeField] private Gradient color = null;
    [SerializeField] private AnimationCurve intensity = null;
    [SerializeField] private AnimationCurve volumetricsMultiplier = null;
    [SerializeField] private AnimationCurve xAngle = null;
    [SerializeField] private AnimationCurve yAngle = null;
    
    public Gradient Color => color;
    public AnimationCurve Intensity => intensity;
    public AnimationCurve VolumetricsMultiplier => volumetricsMultiplier;
    public AnimationCurve XAngle => xAngle;
    public AnimationCurve YAngle => yAngle;
}

public class DynamicLighting : Singleton<DynamicLighting>
{
    [SerializeField] private HDAdditionalLightData mainLight = null;
    [SerializeField] private HDAdditionalLightData fillLight = null;
    
    [SerializeField] private DynamicLightingData mainLightData = null;
    [SerializeField] private DynamicLightingData fillLightData = null;

    private float yRotation;
    
    private void Start()
    {
        RoomSaveData currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        
        yRotation = OrientationUtils.GetYRotation(currentRoomSaveData.CameraOrientation);

        InitialiseLights(GetDepthProportion(currentRoomSaveData.Depth, MapGenerationLookup.Instance.MaxRoomDepth));
    }

    public void InitialiseLights(float depthProportion)
    {
        InitialiseLight(mainLight, mainLightData, depthProportion);
        InitialiseLight(fillLight, fillLightData, depthProportion);
    }

    private float GetDepthProportion(int currentDepth, int maxDepth)
    {
        return (float) (currentDepth - 1) / (maxDepth - 1);
    }
    
    private void InitialiseLight(HDAdditionalLightData light, DynamicLightingData data, float depthProportion)
    {
        light.color = data.Color.Evaluate(depthProportion);
        light.intensity = data.Intensity.Evaluate(depthProportion);
        light.volumetricDimmer = data.VolumetricsMultiplier.Evaluate(depthProportion);
        light.transform.rotation = Quaternion.Euler(
            data.XAngle.Evaluate(depthProportion),
            data.YAngle.Evaluate(depthProportion) + yRotation,
            0
        );
    }
}
