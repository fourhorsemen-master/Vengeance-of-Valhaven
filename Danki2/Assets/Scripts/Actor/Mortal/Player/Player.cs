using System;
using System.Collections;
using System.Collections.Generic;
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
    private CastingStatus _castingStatus = CastingStatus.Ready;
    private float _remainingCooldown = 0f;
    private float _maximumCooldown = 1f;
    private ActionControlState _previousActionControlState = ActionControlState.None;

    public override void Start()
    {
        base.Start();
        _abilityTree = AbilityTreeFactory.CreateTree(
            AbilityTreeFactory.CreateNode(AbilityReference.Slash),
            AbilityTreeFactory.CreateNode(
                AbilityReference.Whirlwind,
                AbilityTreeFactory.CreateNode(AbilityReference.Slash),
                AbilityTreeFactory.CreateNode(AbilityReference.Slash)
            )
        );
    }

    public override void Update()
    {
        _remainingCooldown = Mathf.Max(0f, _remainingCooldown - Time.deltaTime);
        if (_remainingCooldown == 0f && _castingStatus == CastingStatus.Cooldown)
        {
            _castingStatus = CastingStatus.Ready;
        }

        base.Update();
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

        if (_moveDirection != Vector3.zero)
        {
            MoveAlongVector(_moveDirection);
        }
    }

    private void HandleAbilities()
    {
        var _currentControlState = this.GetCurrentActionControlState();
        // handle abilities

        this._previousActionControlState = _currentControlState;
    }

    private ActionControlState GetCurrentActionControlState()
    {
        var left = (Input.GetAxis("Left Action") > 0);
        var right = (Input.GetAxis("Right Action") > 0);

        return left
            ? (right ? ActionControlState.Both : ActionControlState.Left)
            : (right ? ActionControlState.Right : ActionControlState.None);
    }
}
