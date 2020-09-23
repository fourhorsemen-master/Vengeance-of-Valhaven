using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Player _player = null;

    private void Update()
    {
        if (GameStateController.Instance.GameState == GameState.Playing)
        {
            HandleMovement();
            HandleAbilities();
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetAxis("Horizontal") > 0) moveDirection.x += 1f;
        if (Input.GetAxis("Horizontal") < 0) moveDirection.x -= 1f;
        if (Input.GetAxis("Vertical") > 0) moveDirection.z += 1f;
        if (Input.GetAxis("Vertical") < 0) moveDirection.z -= 1f;

        if (Input.GetAxis("Roll") > 0 && moveDirection != Vector3.zero)
        {
            _player.Roll(moveDirection);
        }

        _player.MovementManager.Move(moveDirection);
    }

    private void HandleAbilities()
    {
        bool left = Input.GetAxis("Left Action") > 0;
        bool right = Input.GetAxis("Right Action") > 0;

        ActionControlState currentControlState = left
            ? (right ? ActionControlState.Both : ActionControlState.Left)
            : (right ? ActionControlState.Right : ActionControlState.None);

        _player.AbilityManager.SetCurrentControlState(currentControlState);
    }
}
