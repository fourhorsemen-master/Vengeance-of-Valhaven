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

public class Player : Mortal
{
    private AbilityTree _abilityTree;
    private ChannelService _channelService;
    private CastingStatus _castingStatus = CastingStatus.Ready;
    private ActionControlState _previousActionControlState = ActionControlState.None;
    private float _remainingCooldown = 0f;
    private readonly float _maximumCooldown = 1f;
    private float _remainingDashCooldown = 0f;
    private float _totalDashCooldown = 1f;

    protected override void Start()
    {
        base.Start();

        _movementManager = gameObject.GetComponent<MovementManager>();
        _statsManager = gameObject.GetComponent<StatsManager>();

        _channelService = new ChannelService();

        _abilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                AbilityReference.Fireball,
                AbilityTreeFactory.CreateNode(AbilityReference.ShieldBash),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            ),
            AbilityTreeFactory.CreateNode(
                AbilityReference.Whirlwind,
                AbilityTreeFactory.CreateNode(AbilityReference.Slash),
                AbilityTreeFactory.CreateNode(AbilityReference.ShieldBash)
            )
        );
    }

    protected override void Update()
    {
        base.Update();

        TickDashCooldown();
        TickAbilityCooldown();

        _channelService.Update();
    }

    private void TickAbilityCooldown()
    {
        _remainingCooldown = Mathf.Max(0f, _remainingCooldown - Time.deltaTime);
        if (_remainingCooldown == 0f && _castingStatus == CastingStatus.Cooldown)
        {
            _castingStatus = CastingStatus.Ready;
        }
    }

    private void TickDashCooldown()
    {
        _remainingDashCooldown = Mathf.Max(0f, _remainingDashCooldown - Time.deltaTime);
    }

    protected override void Act()
    {
        this.Move();
        this.HandleAbilities();
    }

    private void Move()
    {
        var _moveDirection = Vector3.zero;

        if (Input.GetAxis("Horizontal") > 0) _moveDirection.x += 1f;
        if (Input.GetAxis("Horizontal") < 0) _moveDirection.x -= 1f;
        if (Input.GetAxis("Vertical") > 0) _moveDirection.z += 1f;
        if (Input.GetAxis("Vertical") < 0) _moveDirection.z -= 1f;

        if (Input.GetAxis("Dash") > 0 && _remainingDashCooldown <= 0)
        {
            _movementManager.LockMovement(0.2f, _statsManager[Stat.Speed] * 3, _moveDirection);
            _remainingDashCooldown = _totalDashCooldown;
        } 
        else
        {
            _movementManager.MoveAlong(_moveDirection);
        }
    }

    private void HandleAbilities()
    {
        var currentControlState = this.GetCurrentActionControlState();

        var castingCommand = ControlMatrix.GetCastingCommand(_castingStatus, _previousActionControlState, currentControlState);

        switch (castingCommand)
        {
            case CastingCommand.ContinueChannel:
                // Handle case where channel has ended naturally.
                if (!_channelService.Active)
                {
                    _castingStatus = _remainingCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
                }
                break;
            case CastingCommand.CancelChannel:
                _channelService.Cancel();
                _castingStatus = _remainingCooldown <= 0f ? CastingStatus.Ready : CastingStatus.Cooldown;
                
                // Ability whiffed, reset tree. TODO: Make a method out of this including feedback for player. 
                _abilityTree.Reset();
                break;
            case CastingCommand.CastLeft:
                this.BranchAndCast(Direction.Left);
                break;
            case CastingCommand.CastRight:
                this.BranchAndCast(Direction.Right);
                break;
        }

        this._previousActionControlState = currentControlState;
    }

    private void BranchAndCast(Direction direction)
    {
        if (!_abilityTree.CanWalk(direction))
        {
            _abilityTree.Reset();
        }

        var abilityReference = _abilityTree.Walk(direction);

        var abilityContext = new AbilityContext(this, MouseGamePositionFinder.Instance.GetMouseGamePosition());

        if (Ability.TryGetAsInstantCastBuilder(abilityReference, out var instantCastbuilder))
        {
            var instantCast = instantCastbuilder.Invoke(abilityContext);
            instantCast.Cast();

            _castingStatus = CastingStatus.Cooldown;
        }

        if (Ability.TryGetAsChannelBuilder(abilityReference, out var channelBuilder))
        {
            var channel = channelBuilder.Invoke(abilityContext);
            _channelService.Start(channel);

            _castingStatus = direction == Direction.Left
                ? CastingStatus.ChannelingLeft
                : CastingStatus.ChannelingRight;
        }

        _remainingCooldown = _maximumCooldown;
    }

    private ActionControlState GetCurrentActionControlState()
    {
        var left = (Input.GetAxis("Left Action") > 0);
        var right = (Input.GetAxis("Right Action") > 0);

        return left
            ? (right ? ActionControlState.Both : ActionControlState.Left)
            : (right ? ActionControlState.Right : ActionControlState.None);
    }
    protected override void OnDeath()
    {
        // TODO: Implement Player death.
        Debug.Log("The player died");
    }
}
