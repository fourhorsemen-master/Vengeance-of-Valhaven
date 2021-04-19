using System.Collections.Generic;

public class TransitionData
{
    public int NextRoomId { get; set; }
    public bool IndicatesNextRoomType { get; set; }
    public List<RoomType> FurtherIndicatedRoomTypes { get; set; }
}
