using System;
using System.Collections.Generic;

public class RoomNode
{
    public RoomNode Parent { get; set; }
    public List<RoomNode> Children { get; set; } = new List<RoomNode>();
    public RoomType RoomType { get; set; }
    public Scene Scene { get; set; }
    public CombatRoomSaveData CombatRoomSaveData { get; set; }  = new CombatRoomSaveData();
    public AbilityRoomSaveData AbilityRoomSaveData { get; set; }  = new AbilityRoomSaveData();
    public HealingRoomSaveData HealingRoomSaveData { get; set; }  = new HealingRoomSaveData();
    public RuneRoomSaveData RuneRoomSaveData { get; set; }  = new RuneRoomSaveData();
    public Dictionary<int, RoomNode> ExitIdToChildLookup { get; } = new Dictionary<int, RoomNode>();
    public Dictionary<RoomNode, int> ChildToExitIdLookup { get; } = new Dictionary<RoomNode, int>();
    public Dictionary<int, bool> ExitIdToIndicatesNextRoomType { get; } = new Dictionary<int, bool>();
    public Dictionary<int, List<RoomType>> ExitIdToFurtherIndicatedRoomTypes { get; } = new Dictionary<int, List<RoomType>>();
    public int ModuleSeed { get; set; }
    public int TransitionModuleSeed { get; set; }
    public Pole CameraOrientation { get; set; }
    public int PlayerSpawnerId { get; set; }
    
    public bool IsRootNode => Parent == null;
    public bool IsLeafNode => Children.Count == 0;

    public int Depth => IsRootNode ? 1 : Parent.Depth + 1;
    
    public void IterateDown(Action<RoomNode> action, Func<RoomNode, bool> filter = null)
    {
        if (filter == null || filter(this)) action(this);
        Children.ForEach(c => c.IterateDown(action, filter));
    }

    public void IterateUp(Action<RoomNode> action, Func<RoomNode, bool> filter = null)
    {
        if (filter == null || filter(this)) action(this);
        if (!IsRootNode) Parent.IterateUp(action, filter);
    }

    public bool HasVictoryRoom()
    {
        bool hasVictoryRoom = false;
        IterateDown(n => hasVictoryRoom = hasVictoryRoom || n.RoomType == RoomType.Victory);
        return hasVictoryRoom;
    }

    /// <summary>
    /// Returns the distance from the nearest parent that has given room type.If no such parent
    /// exists then -1 is returned.
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

    public RoomNode GetRootNode()
    {
        RoomNode rootNode = null;
        IterateUp(node =>
        {
            if (node.IsRootNode) rootNode = node;
        });
        return rootNode;
    }

    public SerializableRoomNode Serialize()
    {
        return null;
    }
}
