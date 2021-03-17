using System;
using System.Collections.Generic;

public class RoomLayoutNode
{
    public RoomLayoutNode Parent { get; set; }
    public List<RoomLayoutNode> Children { get; } = new List<RoomLayoutNode>();

    public int Id { get; set; }
    public RoomType RoomType { get; set; }
    public Scene Scene { get; set; }
    public Pole CameraOrientation { get; set; }
    public int EntranceId { get; set; }
    public Dictionary<int, RoomLayoutNode> ExitIdToChildLookup { get; } = new Dictionary<int, RoomLayoutNode>();
    public Dictionary<RoomLayoutNode, int> ChildToExitIdLookup { get; } = new Dictionary<RoomLayoutNode, int>();
    public Dictionary<int, ActorType> SpawnerIdToSpawnedActor { get; } = new Dictionary<int, ActorType>();

    public bool IsRootNode => Parent == null;
    public bool IsLeafNode => Children.Count == 0;

    public void IterateDown(Action<RoomLayoutNode> action, Func<RoomLayoutNode, bool> filter = null)
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
