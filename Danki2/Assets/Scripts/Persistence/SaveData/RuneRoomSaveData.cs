public class RuneRoomSaveData
{
    public bool RunesViewed { get; set; }
    public bool RuneSelected { get; set; }

    public SerializableRuneRoomSaveData Serialize()
    {
        return new SerializableRuneRoomSaveData
        {
            RunesViewed = RunesViewed,
            RuneSelected = RuneSelected
        };
    }
}
