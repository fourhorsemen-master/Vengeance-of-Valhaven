using Assets.Scripts.KeyMapping;
using UnityEngine;

public class Player : Mortal
{
    private Vector3 _moveDirection = new Vector3();

    protected override void Act()
    {
        Move();
    }

    private void Move()
    { 
        _moveDirection = Vector3.zero;

        if (Input.GetKey(KeyMapper.Mapper.Bindings.Right)) _moveDirection.x += 1f;
        if (Input.GetKey(KeyMapper.Mapper.Bindings.Left)) _moveDirection.x -= 1f;
        if (Input.GetKey(KeyMapper.Mapper.Bindings.Up)) _moveDirection.y += 1f;
        if (Input.GetKey(KeyMapper.Mapper.Bindings.Down)) _moveDirection.y -= 1f;

        MoveAlongVector(_moveDirection);
    }
}
