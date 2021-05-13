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
    
    private void Start()
    {
        RoomNode currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;

        InitialiseLights(GetDepthProportion(currentRoomNode.Depth, MapGenerationLookup.Instance.MaxRoomDepth), currentRoomNode.CameraOrientation);
    }

    public void InitialiseLights(float depthProportion, Pole pole)
    {
        float yRotation = OrientationUtils.GetYRotation(pole);

        InitialiseLight(mainLight, mainLightData, depthProportion, yRotation);
        InitialiseLight(fillLight, fillLightData, depthProportion, yRotation);
    }
    
    private void InitialiseLight(HDAdditionalLightData light, DynamicLightingData data, float depthProportion, float yRotation)
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

    private float GetDepthProportion(int currentDepth, int maxDepth)
    {
        return (float) (currentDepth - 1) / (maxDepth - 1);
    }
}
