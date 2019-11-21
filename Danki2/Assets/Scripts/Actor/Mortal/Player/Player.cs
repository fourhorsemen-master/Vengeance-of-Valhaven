using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mortal
{
    private AbilityTree _abilityTree;

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

    protected override void Act()
    {
        this.Move();
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
}
