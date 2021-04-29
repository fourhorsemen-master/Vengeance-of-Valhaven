public class RuneRoomSaveData
{
    public int Seed { get; set; }
    public bool RunesViewed { get; set; }
    public bool RuneSelected { get; set; }

    public SerializableRuneRoomSaveData Serialize()
    {
        return new SerializableRuneRoomSaveData
        {
            Seed = Seed,
            RunesViewed = RunesViewed,
            RuneSelected = RuneSelected
        };
    }
}
