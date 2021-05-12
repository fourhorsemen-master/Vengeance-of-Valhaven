using System.Collections.Generic;

public class SaveData2
{
    public int Version { get; set; }
    public int Seed { get; set; }

    public int PlayerHealth { get; set; }
    public SerializableAbilityTree SerializableAbilityTree { get; set; }
    public List<RuneSocket> RuneSockets { get; set; }
    public List<Rune> RuneOrder { get; set; }

    public RoomNode CurrentRoomNode { get; set; }
    public RoomNode DefeatRoom { get; set; }

    public SerializableSaveData Serialize()
    {
        return new SerializableSaveData();
    }
}
