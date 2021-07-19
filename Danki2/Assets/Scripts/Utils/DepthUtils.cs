public static class DepthUtils
{
    public static float GetDepthProportion(RoomNode currentRoomNode)
    {
        int currentDepth = currentRoomNode.DepthInZone;
        int maxDepth = MapGenerationLookup.Instance.RoomsPerZoneLookup[currentRoomNode.Zone];

        return (float) (currentDepth - 1) / (maxDepth - 1);
    }
}
