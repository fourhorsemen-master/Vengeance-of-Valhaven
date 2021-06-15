using System;
using System.Collections.Generic;
using System.Linq;

public class RoomNode
{
    public RoomNode Parent { get; set; }
    public List<RoomNode> Children { get; } = new List<RoomNode>();
    public int Depth { get; set; }
    public Zone Zone { get; set; }
    public int DepthInZone { get; set; }
    public RoomType RoomType { get; set; }
    public Scene Scene { get; set; }
    public CombatRoomSaveData CombatRoomSaveData { get; set; }  = new CombatRoomSaveData();
    public AbilityRoomSaveData AbilityRoomSaveData { get; set; }  = new AbilityRoomSaveData();
    public HealingRoomSaveData HealingRoomSaveData { get; set; }  = new HealingRoomSaveData();
    public RuneRoomSaveData RuneRoomSaveData { get; set; }  = new RuneRoomSaveData();
    public Dictionary<int, RoomNode> ExitIdToChildLookup { get; set; } = new Dictionary<int, RoomNode>();
    public Dictionary<RoomNode, int> ChildToExitIdLookup { get; set; } = new Dictionary<RoomNode, int>();
    public Dictionary<int, bool> ExitIdToIndicatesNextRoomType { get; set; } = new Dictionary<int, bool>();
    public Dictionary<int, List<RoomType>> ExitIdToFurtherIndicatedRoomTypes { get; set; } = new Dictionary<int, List<RoomType>>();
    public int ModuleSeed { get; set; }
    public int TransitionModuleSeed { get; set; }
    public Pole CameraOrientation { get; set; }
    public int PlayerSpawnerId { get; set; }
    
    public bool IsRootNode => Parent == null;
    public bool IsLeafNode => Children.Count == 0;
    
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
    /// Returns the distance from the nearest parent that either has OR indicataes one of the given room types. If no such parent
    /// exists then -1 is returned.
    /// </summary>
    public int GetDistanceFromPreviousRoomTypes(params RoomType[] roomTypes)
    {
        bool foundSameRoomType = false;
        int distance = 0;

        IterateUp(
            node =>
            {
                distance++;

                bool hasMatchingRoomType = roomTypes.Contains(node.RoomType);
                bool indicatesMatchingRoomType = node.GetIndicatedRoomTypes().Intersect(roomTypes).Any();

                if (hasMatchingRoomType || (indicatesMatchingRoomType && !node.Equals(Parent)))
                {
                    foundSameRoomType = true;
                }
            },
            node => !foundSameRoomType && !node.Equals(this)
        );

        return foundSameRoomType ? distance : -1;
    }

    public RoomType[] GetIndicatedRoomTypes()
    {
        return ExitIdToIndicatesNextRoomType
            .Where(kvp => kvp.Value)
            .Select(kvp => ExitIdToChildLookup[kvp.Key].RoomType)
            .Distinct()
            .ToArray();
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
        return new SerializableRoomNode
        {
            Depth = Depth,
            Zone = Zone,
            DepthInZone = DepthInZone,
            RoomType = RoomType,
            Scene = Scene,
            SerializableCombatRoomSaveData = CombatRoomSaveData?.Serialize(),
            SerializableAbilityRoomSaveData = AbilityRoomSaveData?.Serialize(),
            SerializableHealingRoomSaveData = HealingRoomSaveData?.Serialize(),
            SerializableRuneRoomSaveData = RuneRoomSaveData?.Serialize(),
            ModuleSeed = ModuleSeed,
            TransitionModuleSeed = TransitionModuleSeed,
            CameraOrientation = CameraOrientation,
            PlayerSpawnerId = PlayerSpawnerId
        };
    }
}
