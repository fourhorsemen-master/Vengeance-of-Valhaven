using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Player _player;


    private void Update()
    {
        Move();
        HandleAbilities();
    }

    private void Move()
    {
        var moveDirection = Vector3.zero;

        if (Input.GetAxis("Horizontal") > 0) moveDirection.x += 1f;
        if (Input.GetAxis("Horizontal") < 0) moveDirection.x -= 1f;
        if (Input.GetAxis("Vertical") > 0) moveDirection.z += 1f;
        if (Input.GetAxis("Vertical") < 0) moveDirection.z -= 1f;

        if (Input.GetAxis("Dash") > 0)
        {
            _player.Dash(moveDirection);
        }
        else
        {
            _player.MoveAlong(moveDirection);
        }
    }

    private void HandleAbilities()
    {
        var currentControlState = GetCurrentActionControlState();
        _player.SetCurrentControlState(currentControlState);
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