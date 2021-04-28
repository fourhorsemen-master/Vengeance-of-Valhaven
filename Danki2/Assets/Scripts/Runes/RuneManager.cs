using System.Collections.Generic;

public class RuneManager
{
    public List<Rune> Runes { get; }
    public Subject<Rune> RuneAddedSubject { get; } = new Subject<Rune>();

    public RuneManager(Player player)
    {
        Runes = PersistenceManager.Instance.SaveData.Runes;

        player.StatsManager.RegisterPipe(new IronSkinHandler(this, player));
    }

    public bool HasRune(Rune rune) => Runes.Contains(rune);
    
    public void AddRune(Rune rune)
    {
        Runes.Add(rune);
        RuneAddedSubject.Next(rune);
    }
}
