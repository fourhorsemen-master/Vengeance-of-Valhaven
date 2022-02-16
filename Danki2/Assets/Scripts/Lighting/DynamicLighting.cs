using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DynamicLighting : Singleton<DynamicLighting>
{
    [SerializeField] private HDAdditionalLightData mainLight = null;
    [SerializeField] private HDAdditionalLightData fillLight = null;
    
    [SerializeField] private DynamicLightingConfig zone1MainLightConfig = null;
    [SerializeField] private DynamicLightingConfig zone2MainLightConfig = null;
    [SerializeField] private DynamicLightingConfig zone3MainLightConfig = null;

    [SerializeField] private DynamicLightingConfig zone1FillLightConfig = null;
    [SerializeField] private DynamicLightingConfig zone2FillLightConfig = null;
    [SerializeField] private DynamicLightingConfig zone3FillLightConfig = null;

    private Dictionary<Zone, DynamicLightingConfig> mainLightConfigLookup = null;
    private Dictionary<Zone, DynamicLightingConfig> fillLightConfigLookup = null;

    private void Start()
    {
        RoomNode currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;

        mainLightConfigLookup = new Dictionary<Zone, DynamicLightingConfig>
        {
            { Zone.Zone1, zone1MainLightConfig },
            { Zone.Zone2, zone2MainLightConfig },
            { Zone.Zone3, zone3MainLightConfig }
        };

        fillLightConfigLookup = new Dictionary<Zone, DynamicLightingConfig>
        {
            { Zone.Zone1, zone1FillLightConfig },
            { Zone.Zone2, zone2FillLightConfig },
            { Zone.Zone3, zone3FillLightConfig }
        };

        InitialiseLights(
            DepthUtils.GetDepthProportion(currentRoomNode),
            currentRoomNode.CameraOrientation
        );
    }

    public void InitialiseLights(float depthProportion, Pole pole)
    {
        Zone zone = PersistenceManager.Instance.SaveData.CurrentRoomNode.Zone;

        float yRotation = OrientationUtils.GetYRotation(pole);

        InitialiseLight(mainLight, mainLightConfigLookup[zone], depthProportion, yRotation);
        InitialiseLight(fillLight, fillLightConfigLookup[zone], depthProportion, yRotation);
    }
    
    private void InitialiseLight(HDAdditionalLightData light, DynamicLightingConfig data, float depthProportion, float yRotation)
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
