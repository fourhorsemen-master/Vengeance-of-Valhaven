﻿using System;
using UnityEngine;
using System.Collections;

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
    public float abilityTimeoutLimit = 5f;

    private float _remainingDashCooldown = 0f;
    private Direction lastCastDirection;
    private bool whiffed = true;
    private bool recievedCastFeedback = false;
    private int castIndex = 0;

    private Coroutine _abilityTimeout = null;

    private ActionControlState _previousActionControlState = ActionControlState.None;
    private ActionControlState _currentActionControlState = ActionControlState.None;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    [HideInInspector]
    public float RemainingAbilityCooldown { get; private set; } = 0f;
    [HideInInspector]
    public CastingStatus CastingStatus { get; private set; } = CastingStatus.Ready;

    public AbilityTree AbilityTree { get; private set; }

    public Subject<Tuple<bool, Direction>> AbilityCompletionSubject { get; } = new Subject<Tuple<bool, Direction>>();

    public override ActorType Type => ActorType.Player;

    protected override void Awake()
    {
        base.Awake();

        AbilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(
                AbilityReference.Slash,
                AbilityTreeFactory.CreateNode(
                    AbilityReference.Roll,
                    rightChild: AbilityTreeFactory.CreateNode(AbilityReference.Smash)
                ),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            ),
            AbilityTreeFactory.CreateNode(
                AbilityReference.Lunge,
                AbilityTreeFactory.CreateNode(AbilityReference.DaggerThrow),
                AbilityTreeFactory.CreateNode(AbilityReference.Whirlwind)
            )
        );
    }

    protected override void Start()
    {
        base.Start();

        this.gameObject.tag = Tags.Player;

        AbilityTimeoutSubscription();
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

    public void Dash(Vector3 direction)
    {
        if (_remainingDashCooldown <= 0)
        {
            LockMovement(dashDuration, GetStat(Stat.Speed) * dashSpeedMultiplier, direction);
            _remainingDashCooldown = totalDashCooldown;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashVisualAfterDelay());
        }
    }

    public void SetCurrentControlState(ActionControlState controlState)
    {
        _currentActionControlState = controlState;
    }

    public void SubscribeToTreeWalk(Action<Node> callback)
    {
        AbilityTree.TreeWalkSubject.Subscribe(callback);
    }

    protected override void OnDeath()
    {
        // TODO: Implement Player death.
        Debug.Log("The player died");
    }

    private void AbilityTimeoutSubscription()
    {
        AbilityTree.CurrentDepthSubject.Subscribe((int treeDepth) =>
        {
            if (_abilityTimeout != null)
            {
                StopCoroutine(_abilityTimeout);
            }

            if (treeDepth > 0)
            {
                _abilityTimeout = StartCoroutine(AbilityTimeoutCounter());
            }
        });
    }

    private IEnumerator AbilityTimeoutCounter()
    {
        yield return new WaitForSeconds(abilityTimeoutLimit);
        AbilityTree.Reset();
        this.whiffed = true;
        this.recievedCastFeedback = false;
    }

    private void TickDashCooldown()
    {
        _remainingDashCooldown = Mathf.Max(0f, _remainingDashCooldown - Time.deltaTime);
    }

    private void TickAbilityCooldown()
    {
        if (_channelService.Active) return;

        RemainingAbilityCooldown = Mathf.Max(0f, RemainingAbilityCooldown - Time.deltaTime);

        if (RemainingAbilityCooldown > 0f || CastingStatus != CastingStatus.Cooldown) return;

        CastingStatus = CastingStatus.Ready;
        AbilityTree.Walk(this.lastCastDirection);

        if (!AbilityTree.CanWalk() || this.whiffed)
        {
            AbilityTree.Reset();
        }

        this.whiffed = true;
        this.recievedCastFeedback = false;
    }

    private IEnumerator EndDashVisualAfterDelay()
    {
        yield return new WaitForSeconds(dashDuration * 2);
        trailRenderer.emitting = false;
    }

    private void HandleAbilities()
    {
        CastingCommand castingCommand = ControlMatrix.GetCastingCommand(
            CastingStatus,
            _previousActionControlState,
            _currentActionControlState
        );

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
                break;
            case CastingCommand.CastLeft:
                BranchAndCast(Direction.Left);
                break;
            case CastingCommand.CastRight:
                BranchAndCast(Direction.Right);
                break;
        }
    }

    private void BranchAndCast(Direction direction)
    {
        if (!AbilityTree.CanWalkDirection(direction))
        {
            // Feedback to user that there is no ability here.
            return;
        }

        this.castIndex += 1;

        RemainingAbilityCooldown = abilityCooldown;
        this.lastCastDirection = direction;
        if (_abilityTimeout != null)
        {
            StopCoroutine(_abilityTimeout);
        }

        AbilityReference abilityReference = AbilityTree.GetAbility(direction);

        AbilityContext abilityContext = new AbilityContext(
            this,
            MouseGamePositionFinder.Instance.GetMouseGamePosition()
        );

        if (Ability.TryGetAsInstantCastBuilder(abilityReference, out var instantCastbuilder))
        {
            InstantCast instantCast = instantCastbuilder(abilityContext, AbilitySuccessCallback(this.castIndex));
            instantCast.Cast();

            CastingStatus = CastingStatus.Cooldown;
        }

        if (Ability.TryGetAsChannelBuilder(abilityReference, out var channelBuilder))
        {
            Channel channel = channelBuilder(abilityContext, AbilitySuccessCallback(this.castIndex));
            _channelService.Start(channel);

            CastingStatus = direction == Direction.Left
                ? CastingStatus.ChannelingLeft
                : CastingStatus.ChannelingRight;
        }
    }

    private Action<bool> AbilitySuccessCallback(int castIndex)
    {
        return successful =>
        {
            if (castIndex == this.castIndex && !recievedCastFeedback)
            {
                this.recievedCastFeedback = true;
                this.whiffed = !successful;
                this.AbilityCompletionSubject.Next(
                    new Tuple<bool, Direction>(successful, this.lastCastDirection)
                );
            }
        };
    }
}
