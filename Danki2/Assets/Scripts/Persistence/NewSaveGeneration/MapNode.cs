using System;
using System.Collections.Generic;

public class MapNode
{
    public MapNode Parent { get; set; }
    public List<MapNode> Children { get; } = new List<MapNode>();

    public int Id { get; set; }
    public RoomType RoomType { get; set; }
    public Scene Scene { get; set; }
    public Pole CameraOrientation { get; set; }
    public int EntranceId { get; set; }
    public Dictionary<int, MapNode> ExitIdToChildLookup { get; } = new Dictionary<int, MapNode>();
    public Dictionary<MapNode, int> ChildToExitIdLookup { get; } = new Dictionary<MapNode, int>();
    public Dictionary<int, bool> ExitIdToIndicatesNextRoomType { get; } = new Dictionary<int, bool>();
    public Dictionary<int, List<RoomType>> ExitIdToFurtherIndicatedRoomTypes { get; } = new Dictionary<int, List<RoomType>>();
    public Dictionary<int, ActorType> SpawnerIdToSpawnedActor { get; } = new Dictionary<int, ActorType>();
    public List<AbilityReference> AbilityChoices { get; } = new List<AbilityReference>();

    public bool IsRootNode => Parent == null;
    public bool IsLeafNode => Children.Count == 0;

    public int Depth => IsRootNode ? 1 : Parent.Depth + 1;

    public void IterateDown(Action<MapNode> action, Func<MapNode, bool> filter = null)
    {
        if (filter == null || filter(this)) action(this);
        Children.ForEach(c => c.IterateDown(action, filter));
    }

    public void IterateUp(Action<MapNode> action, Func<MapNode, bool> filter = null)
    {
        if (filter == null || filter(this)) action(this);
        if (!IsRootNode) Parent.IterateUp(action, filter);
    }

    public int FindMaxId()
    {
        int maxId = Id;
        IterateDown(n => maxId = n.Id > maxId ? n.Id : maxId);
        return maxId;
    }

    /// <summary>
    /// Returns the distance from the nearest parent that has given room type. If no such parent exists then -1 is returned.
    /// </summary>
    public int GetDistanceFromPreviousRoomType(RoomType roomType)
    {
        bool foundSameRoomType = false;
        int distance = 0;

        IterateUp(
            node =>
            {
                distance++;
                if (node.RoomType == roomType) foundSameRoomType = true;
            },
            node => !foundSameRoomType && !node.Equals(this)
        );

        return foundSameRoomType ? distance : -1;
    }
}
