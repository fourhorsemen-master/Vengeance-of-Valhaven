using System.Collections.Generic;

public class RuneManager
{
	private const int RuneLimit = 3;

	private readonly Player player;

	private readonly HashSet<Rune> runes = new HashSet<Rune>();

	/// <summary>
	/// In this implemntation:
	/// - no more than 3 runes can be added
	/// - no duplicates allowed
	/// - no removing runes either.
	/// </summary>
	/// <param name="player"></param>
	public RuneManager(Player player)
	{
		this.player = player;
	}

	public bool HasRune(Rune rune) => runes.Contains(rune);

	// No more than 3 runes - and no duplicates.
	public bool TryAddRune(Rune rune)
	{
		if (runes.Contains(rune) || runes.Count >= RuneLimit) return false;

		runes.Add(rune);
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
				player.HealthManager.ReceiveHeal(player.StatsManager.Get(Stat.MaxHealth));
				break;
		}
	}
}
