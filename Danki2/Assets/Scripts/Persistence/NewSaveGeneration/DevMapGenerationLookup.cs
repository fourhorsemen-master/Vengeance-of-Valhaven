using UnityEngine;

public class DevMapGenerationLookup : MapGenerationLookup
{
    [SerializeField] private int maxRoomDepthOverride = 0;

    public override int MaxRoomDepth { get => maxRoomDepthOverride; set => maxRoomDepthOverride = value; }

    protected override bool DestroyOnLoad => true;
}
