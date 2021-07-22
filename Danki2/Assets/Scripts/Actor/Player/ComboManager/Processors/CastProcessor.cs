using System.Collections.Generic;

public class CastProcessor : Processor<ComboState>
{
    private readonly Player player;
    private readonly Direction castDirection;

    public CastProcessor(Player player, Direction castDirection)
    {
        this.player = player;
        this.castDirection = castDirection;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public bool TryCompleteProcess(out ComboState nextState)
    {
        Ability2 ability = player.AbilityTree.GetAbility(castDirection);
        player.AbilityTree.Walk(castDirection);
        player.AbilityService.Cast(ability, player.TargetFinder.FloorTargetPosition);

        nextState = ComboState.FinishAbility;
        return true;
    }
}
