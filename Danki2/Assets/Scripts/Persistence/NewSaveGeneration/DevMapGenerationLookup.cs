using UnityEngine;

public class DevMapGenerationLookup : MapGenerationLookup
{
    [SerializeField] private int runeSocketsOverride = 0;
    [SerializeField] private int maxRoomDepthOverride = 0;

    public override int RuneSockets { get => runeSocketsOverride; set => runeSocketsOverride = value; }
    public override int MaxRoomDepth { get => maxRoomDepthOverride; set => maxRoomDepthOverride = value; }

    protected override bool DestroyOnLoad => true;
}
