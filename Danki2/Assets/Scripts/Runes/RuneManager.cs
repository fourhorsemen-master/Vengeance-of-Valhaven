using System.Collections.Generic;
using System.Linq;

public class RuneManager
{
    public List<RuneSocket> RuneSockets { get; }
    public Subject<Rune> RuneAddedSubject { get; } = new Subject<Rune>();

    public RuneManager(Player player)
    {
        RuneSockets = PersistenceManager.Instance.SaveData.RuneSockets;

        player.StatsManager.RegisterPipe(new IronSkinHandler(this, player));
        player.StatsManager.RegisterPipe(new FleetOfFootHandler(this, player));
    }

    public bool HasRune(Rune rune) => RuneSockets.Any(s => s.HasRune && s.Rune == rune);
    
    public void AddRune(RuneSocket runeSocket, Rune rune)
    {
        runeSocket.HasRune = true;
        runeSocket.Rune = rune;
        RuneAddedSubject.Next(rune);
    }
}
