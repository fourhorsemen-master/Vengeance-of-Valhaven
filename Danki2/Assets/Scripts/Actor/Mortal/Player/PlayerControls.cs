using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private void Update()
    {
        HandleMovement();
        HandleAbilities();
    }

    private void HandleMovement()
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

        _player.MoveAlong(moveDirection);
    }

    private void HandleAbilities()
    {
        var left = Input.GetAxis("Left Action") > 0;
        var right = Input.GetAxis("Right Action") > 0;

        var currentControlState = left
            ? (right ? ActionControlState.Both : ActionControlState.Left)
            : (right ? ActionControlState.Right : ActionControlState.None);

        _player.SetCurrentControlState(currentControlState);
    }
}
