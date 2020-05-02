using System.Collections.Generic;

public class PlayerTooltipBuilder
{
    private Player player;

    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();

    public PlayerTooltipBuilder(Player player)
    {
        this.player = player;
        differs.Add(new AbilityDataStatsDiffer(player));
        differs.Add(new AbilityDataOrbsDiffer(player.AbilityTree));
    }

    public List<TooltipSegment> Build(Node node)
    {
        return null;
    }
}
