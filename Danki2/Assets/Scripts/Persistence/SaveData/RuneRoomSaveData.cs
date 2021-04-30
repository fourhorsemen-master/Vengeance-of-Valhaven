public class RuneRoomSaveData
{
    public bool HasIncrementedRuneIndex { get; set; }
    public bool RunesViewed { get; set; }
    public bool RuneSelected { get; set; }

    public SerializableRuneRoomSaveData Serialize()
    {
        return new SerializableRuneRoomSaveData
        {
            HasIncrementedRuneIndex = HasIncrementedRuneIndex,
            RunesViewed = RunesViewed,
            RuneSelected = RuneSelected
        };
    }
}
