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
        player.AbilityService.Cast(Ability2.Slash1, player.TargetFinder.FloorTargetPosition);

        nextState = ComboState.FinishAbility;

        return true;
        // AbilityReference abilityReference = player.AbilityTree.GetAbility(castDirection);
        // AbilityType abilityType = AbilityLookup.Instance.GetAbilityType(abilityReference);
        //
        // switch (abilityType)
        // {
        //     case AbilityType.InstantCast:
        //         player.AbilityTree.Walk(castDirection);
        //         player.InstantCastService.Cast(
        //             abilityReference,
        //             player.TargetFinder.FloorTargetPosition,
        //             player.TargetFinder.OffsetTargetPosition,
        //             player.TargetFinder.Target
        //         );
        //         nextState = ComboState.FinishAbility;
        //         return true;
        //
        //     case AbilityType.Channel:
        //         player.AbilityTree.Walk(castDirection);
        //         player.ChannelService.StartChannel(abilityReference);
        //         nextState =  ComboState.Channeling;
        //         return true;
        //
        //     default:
        //         // This can never happen
        //         nextState = default;
        //         return false;
        // }
    }
}
