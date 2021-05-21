public class RuneRoomSaveData
{
    public bool RunesViewed { get; set; } = false;
    public bool RuneSelected { get; set; } = false;

    public SerializableRuneRoomSaveData Serialize()
    {
        return new SerializableRuneRoomSaveData
        {
            RunesViewed = RunesViewed,
            RuneSelected = RuneSelected
        };
    }
}
