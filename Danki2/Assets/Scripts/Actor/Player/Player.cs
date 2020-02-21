using System;
using UnityEngine;

public enum CastingStatus
{
    ChannelingLeft,
    ChannelingRight,
    Cooldown,
    Ready
}

public enum CastingCommand
{
    ContinueChannel,
    CancelChannel,
    CastLeft,
    CastRight,
    None
}

public enum ActionControlState
{
    Left,
    Right,
    Both,
    None
}

public class Player : Actor
{
    [HideInInspector]
    public float abilityCooldown = 1f;
    [HideInInspector]
    public float totalDashCooldown = 1f;
    [HideInInspector]
    public float dashDuration = 0.2f;
    [HideInInspector]
    public float dashSpeedMultiplier = 3f;

    [HideInInspector]
    public float RemainingAbilityCooldown { get; private set; } = 0f;
    [HideInInspector]
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;

    private float _remainingDashCooldown = 0f;
    private AbilityTree _abilityTree;
    
    private ActionControlState _previousActionControlState = ActionControlState.None;
    private ActionControlState _currentActionControlState = ActionControlState.None;

    public override ActorType Type => ActorType.Player;

    protected override void Awake()
    {
        base.Awake();

        _abilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                AbilityReference.Slash,
                AbilityTreeFactory.CreateNode(
                    AbilityReference.Roll,
                    rightChild: AbilityTreeFactory.CreateNode(AbilityReference.DaggerThrow)
                ),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            ),
            AbilityTreeFactory.CreateNode(
                AbilityReference.Lunge,
                AbilityTreeFactory.CreateNode(AbilityReference.Slash),
                AbilityTreeFactory.CreateNode(AbilityReference.Roll)
            )
        );
    }

    protected override void Start()
    {
        base.Start();

        this.gameObject.tag = Tags.Player;
    }

    protected override void Update()
    {
        base.Update();

        TickDashCooldown();
        TickAbilityCooldown();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        HandleAbilities();
    }

    private void TickDashCooldown()
    {
        _remainingDashCooldown = Mathf.Max(0f, _remainingDashCooldown - Time.deltaTime);
    }

    private void TickAbilityCooldown()
    {
        RemainingAbilityCooldown = Mathf.Max(0f, RemainingAbilityCooldown - Time.deltaTime);
        if (RemainingAbilityCooldown == 0f && CastingStatus == CastingStatus.Cooldown)
        {
            CastingStatus = CastingStatus.Ready;
        }
    }

    public void Dash(Vector3 direction)
    {
        if (_remainingDashCooldown <= 0)
        {
            LockMovement(dashDuration, GetStat(Stat.Speed) * dashSpeedMultiplier, direction);
            _remainingDashCooldown = totalDashCooldown;
        }
    }

    public void SetCurrentControlState(ActionControlState controlState)
    {
        _currentActionControlState = controlState;
    }

    private void HandleAbilities()
    {
        var castingCommand = ControlMatrix.GetCastingCommand(CastingStatus, _previousActionControlState, _currentActionControlState);
        _previousActionControlState = _currentActionControlState;
        _currentActionControlState = ActionControlState.None;

        switch (castingCommand)
        {
            case CastingCommand.ContinueChannel:
                // Handle case where channel has ended naturally.
                if (!_channelService.Active)
                {
                    CastingStatus = RemainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
                }
                break;
            case CastingCommand.CancelChannel:
                _channelService.Cancel();
                CastingStatus = RemainingAbilityCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;

                // Ability whiffed, reset tree. TODO: Make a method out of this including feedback for player. 
                _abilityTree.Reset();
                break;
            case CastingCommand.CastLeft:
                BranchAndCast(Direction.Left);
                break;
            case CastingCommand.CastRight:
                BranchAndCast(Direction.Right);
                break;
        }
    }

    public void SubscribeToTreeWalk(Action<Node> callback)
    {
        _abilityTree.TreeWalkSubject.Subscribe(callback);
    }

    private void BranchAndCast(Direction direction)
    {
        RemainingAbilityCooldown = abilityCooldown;

        if (!_abilityTree.CanWalkDirection(direction))
        {
            // Whiffed!
            _abilityTree.Reset();
            return;
        }

        var abilityReference = _abilityTree.Walk(direction);

        var abilityContext = new AbilityContext(this, MouseGamePositionFinder.Instance.GetMouseGamePosition());

        if (Ability.TryGetAsInstantCastBuilder(abilityReference, out var instantCastbuilder))
        {
            var instantCast = instantCastbuilder.Invoke(abilityContext);
            instantCast.Cast();

            CastingStatus = CastingStatus.Cooldown;
        }

        if (Ability.TryGetAsChannelBuilder(abilityReference, out var channelBuilder))
        {
            var channel = channelBuilder.Invoke(abilityContext);
            _channelService.Start(channel);

            CastingStatus = direction == Direction.Left
                ? CastingStatus.ChannelingLeft
                : CastingStatus.ChannelingRight;
        }

        if (!_abilityTree.CanWalk())
        {
            _abilityTree.Reset();
            return;
        }
    }

    protected override void OnDeath()
    {
        // TODO: Implement Player death.
        Debug.Log("The player died");
    }
}
