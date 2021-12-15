using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneManager
{
    public List<RuneSocket> RuneSockets { get; }
    public Subject<Rune> RuneAddedSubject { get; } = new Subject<Rune>();
    public Subject<Rune> RuneRemovedSubject { get; } = new Subject<Rune>();

    public RuneManager(Player player)
    {
        RuneSockets = PersistenceManager.Instance.SaveData.RuneSockets;

        player.RegisterMaxHealthPipe(IronSkinHandler.ProcessMaxHealth);
        player.RegisterSpeedPipe(FleetOfFootHandler.ProcessSpeed);
    }

    public bool HasRune(Rune rune) => RuneSockets.Any(s => s.HasRune && s.Rune == rune);
    
    public void AddRune(RuneSocket runeSocket, Rune rune)
    {
        if (runeSocket.HasRune) RemoveRune(runeSocket);

        runeSocket.HasRune = true;
        runeSocket.Rune = rune;
        RuneAddedSubject.Next(rune);
    }

    public void RemoveRune(RuneSocket runeSocket)
    {
        Rune previousRune = runeSocket.Rune;
        runeSocket.HasRune = false;
        runeSocket.Rune = default;
        RuneRemovedSubject.Next(previousRune);
    }

    public Rune GetNextRune()
    {
        List<Rune> runeOrder = PersistenceManager.Instance.SaveData.RuneOrder;

        int runesViewed = GetRunesViewed();
        
        for (int i = 0; i < runeOrder.Count; i ++)
        {
            Rune rune = runeOrder[(runesViewed + i) % runeOrder.Count];
            if (!RuneSockets.Any(s => s.HasRune && s.Rune == rune)) return rune;
        }

        Debug.LogError("No suitable Runes found");
        return default;
    }

    //TODO: Include runes from shops here.
    private int GetRunesViewed()
    {
        RoomNode currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;
        int runesViewed = 0;
        
        currentRoomNode.IterateUp(
            node => runesViewed += node.RoomType == RoomType.Rune ? 1 : 0,
            node => node != currentRoomNode
        );
        
        return runesViewed;
    }
}
