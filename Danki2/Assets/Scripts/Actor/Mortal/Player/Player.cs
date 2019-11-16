using Assets.Scripts.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mortal
{
    public override IAI AI => null;

    protected override void Act()
    {
        this.Move();
    }

    private void Move()
    {
        var _moveDirection = Vector3.zero;

        if (Input.GetAxis("Horizontal") > 0) _moveDirection.x += 1f;
        if (Input.GetAxis("Horizontal") < 0) _moveDirection.x -= 1f;
        if (Input.GetAxis("Vertical") > 0) _moveDirection.y += 1f;
        if (Input.GetAxis("Vertical") < 0) _moveDirection.y -= 1f;

        MoveAlongVector(_moveDirection);
    }
}
