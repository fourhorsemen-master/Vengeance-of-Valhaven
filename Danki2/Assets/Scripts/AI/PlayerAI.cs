using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : Player
{
    [SerializeField]
    private Actor _actor;

    private float _remainingDashCooldown = 0f;

    protected override void Update()
    {
        TickDashCooldown();
        Move();
    }

    private void TickDashCooldown()
    {
        _remainingDashCooldown = Mathf.Max(0f, _remainingDashCooldown - Time.deltaTime);
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
            
            LockMovement(dashDuration, GetStat(Stat.Speed) * dashSpeedMultiplier, _moveDirection);
            _remainingDashCooldown = totalDashCooldown;
        }
        else
        {
            MoveAlong(_moveDirection);
        }
    }
}
