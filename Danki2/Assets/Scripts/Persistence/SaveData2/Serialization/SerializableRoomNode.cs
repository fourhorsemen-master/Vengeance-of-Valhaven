using System.Collections.Generic;
using UnityEngine;

public class SerializableRoomNode
{
    [SerializeField] private int id;
    [SerializeField] private int parentId;
    [SerializeField] private List<int> childrenIds;
    [SerializeField] private RoomType roomType;
    [SerializeField] private Scene scene;
    [SerializeField] private SerializableCombatRoomSaveData serializableCombatRoomSaveData;
    [SerializeField] private SerializableAbilityRoomSaveData serializableAbilityRoomSaveData;
    [SerializeField] private SerializableHealingRoomSaveData serializableHealingRoomSaveData;
    [SerializeField] private SerializableRuneRoomSaveData serializableRuneRoomSaveData;
    // TODO: Add transition data here and serialize and deserialize properly...
    [SerializeField] private int moduleSeed;
    [SerializeField] private int transitionModuleSeed;
    [SerializeField] private Pole cameraOrientation;
    [SerializeField] private int playerSpawnerId;
    
    public int Id { get => id; set => id = value; }
    public int ParentId { get => parentId; set => parentId = value; }
    public List<int> ChildrenIds { get => childrenIds; set => childrenIds = value; }
    public RoomType RoomType { get => roomType; set => roomType = value; }
    public Scene Scene { get => scene; set => scene = value; }
    public SerializableCombatRoomSaveData SerializableCombatRoomSaveData { get => serializableCombatRoomSaveData; set => serializableCombatRoomSaveData = value; }
    public SerializableAbilityRoomSaveData SerializableAbilityRoomSaveData { get => serializableAbilityRoomSaveData; set => serializableAbilityRoomSaveData = value; }
    public SerializableHealingRoomSaveData SerializableHealingRoomSaveData { get => serializableHealingRoomSaveData; set => serializableHealingRoomSaveData = value; }
    public SerializableRuneRoomSaveData SerializableRuneRoomSaveData { get => serializableRuneRoomSaveData; set => serializableRuneRoomSaveData = value; }
    // TODO: Add transition data here and serialize and deserialize properly...
    public int ModuleSeed { get => moduleSeed; set => moduleSeed = value; }
    public int TransitionModuleSeed { get => transitionModuleSeed; set => transitionModuleSeed = value; }
    public Pole CameraOrientation { get => cameraOrientation; set => cameraOrientation = value; }
    public int PlayerSpawnerId { get => playerSpawnerId; set => playerSpawnerId = value; }

    public RoomNode Deserialize()
    {
        return null;
    }
}
