using System.Collections.Generic;

public class RuneManager
{
	private readonly Player player;

	private readonly HashSet<Rune> runes = new HashSet<Rune>();

	public RuneManager(Player player)
	{
		this.player = player;
	}

	public bool HasRune(Rune rune) => runes.Contains(rune);

	// Can't have duplicate runes
	public bool TryAddRune(Rune rune)
	{
		if (!runes.Add(rune)) return false;

		Activate(rune);
		return true;
	}

	// No need for a Deactivate method as runes can't be removed
	public void Activate(Rune rune)
	{
		switch (rune)
		{
			case Rune.FleetOfFoot:
				player.StatsManager.RegisterPipe(new FleetOfFootHandler());
				break;
			case Rune.IronSkin:
				player.StatsManager.RegisterPipe(new IronSkinHandler());
				break;
		}
	}
}
