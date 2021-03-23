using System.Collections.Generic;
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
        if (PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomType != RoomType.Ability || AbilitiesViewed) return;

        List<AbilityReference> choices = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.AbilityRoomSaveData.AbilityChoices;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActorCache.Instance.Player.AbilityTree.AddToInventory(choices[0]);
            AbilitiesViewed = true;
            abilitiesViewedSubject.Next();
            PersistenceManager.Instance.Save();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActorCache.Instance.Player.AbilityTree.AddToInventory(choices[1]);
            AbilitiesViewed = true;
            abilitiesViewedSubject.Next();
            PersistenceManager.Instance.Save();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActorCache.Instance.Player.AbilityTree.AddToInventory(choices[2]);
            AbilitiesViewed = true;
            abilitiesViewedSubject.Next();
            PersistenceManager.Instance.Save();
        }
    }
}
