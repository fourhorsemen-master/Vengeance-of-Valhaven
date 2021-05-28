public class ZoneIntroductionRoomManager : Singleton<ZoneIntroductionRoomManager>
{
    private void Start()
    {
        if (PersistenceManager.Instance.SaveData.CurrentRoomNode.RoomType != RoomType.ZoneIntroduction) return;

        Subject canTransitionSubject = new Subject();
        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(canTransitionSubject);
        canTransitionSubject.Next();
    }
}
