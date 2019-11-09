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

        if (Input.GetAxis("Horizontal") > 0) _moveDirection.x += 1f;
        if (Input.GetAxis("Horizontal") < 0) _moveDirection.x -= 1f;
        if (Input.GetAxis("Vertical") > 0) _moveDirection.y += 1f;
        if (Input.GetAxis("Vertical") < 0) _moveDirection.y -= 1f;

        MoveAlongVector(_moveDirection);
    }
}
