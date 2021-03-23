using UnityEngine;

public class AbilityRoomManager : Singleton<AbilityRoomManager>
{
    public bool AbilitiesViewed { get; private set; } = false;

    private readonly Subject abilitiesViewedSubject = new Subject();

    private void Start()
    {
        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (roomSaveData.RoomType != RoomType.Ability) return;

        AbilitiesViewed = roomSaveData.AbilityRoomSaveData.AbilitiesViewed;

        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(abilitiesViewedSubject);

        if (AbilitiesViewed) abilitiesViewedSubject.Next();
    }

    private void Update()
    {
        if (AbilitiesViewed) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            AbilitiesViewed = true;
            abilitiesViewedSubject.Next();
            PersistenceManager.Instance.Save();
        }
    }
}
