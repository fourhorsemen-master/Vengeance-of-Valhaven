using System.Collections.Generic;
using System.Linq;

public class RuneManager
{
    public List<RuneSocket> RuneSockets { get; }
    public int NextRuneIndex { get; private set; }
    public Subject<Rune> RuneAddedSubject { get; } = new Subject<Rune>();
    public Subject<Rune> RuneRemovedSubject { get; } = new Subject<Rune>();

    public RuneManager(Player player)
    {
        RuneSockets = PersistenceManager.Instance.SaveData.RuneSockets;
        NextRuneIndex = PersistenceManager.Instance.SaveData.NextRuneIndex;

        player.StatsManager.RegisterPipe(new IronSkinHandler(this, player));
        player.StatsManager.RegisterPipe(new FleetOfFootHandler(this, player));
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
        Rune nextRune = PersistenceManager.Instance.SaveData.RuneOrder[NextRuneIndex];
        NextRuneIndex = (NextRuneIndex + 1) % PersistenceManager.Instance.SaveData.RuneOrder.Count;
        return nextRune;
    }
}
