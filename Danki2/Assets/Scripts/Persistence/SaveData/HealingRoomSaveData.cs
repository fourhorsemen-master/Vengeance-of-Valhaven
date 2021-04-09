public class HealingRoomSaveData
{
    public bool HasHealed { get; set; } = false;

    public SerializableHealingRoomSaveData Serialize()
    {
        return new SerializableHealingRoomSaveData
        {
            HasHealed = HasHealed
        };
    }
}
