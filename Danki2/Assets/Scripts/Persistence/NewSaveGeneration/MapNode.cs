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

    public int FindMaxId()
    {
        int maxId = Id;
        IterateDown(n => maxId = n.Id > maxId ? n.Id : maxId);
        return maxId;
    }
}
