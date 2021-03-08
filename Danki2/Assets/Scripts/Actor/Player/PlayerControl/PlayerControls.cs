﻿using UnityEngine;

public class PlayerControls : Singleton<PlayerControls>
{
    [SerializeField]
    private Player player = null;

    public ActionControlState ActionControlState { get; private set; } = ActionControlState.None;

    private void Update()
    {
        if (GameplayStateController.Instance.GameplayState == GameplayState.Playing)
        {
            HandleMovement();
            HandleAbilities();
            HandleSceneControls();
        }
    }

    private void HandleMovement()
    {
        if (player.Dead) return;

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetAxis("Horizontal") > 0) moveDirection.x += 1f;
        if (Input.GetAxis("Horizontal") < 0) moveDirection.x -= 1f;
        if (Input.GetAxis("Vertical") > 0) moveDirection.z += 1f;
        if (Input.GetAxis("Vertical") < 0) moveDirection.z -= 1f;

        if (Input.GetAxis("Roll") > 0 && moveDirection != Vector3.zero)
        {
            player.Roll(moveDirection);
        }

        player.MovementManager.Move(moveDirection);
    }

    private void HandleAbilities()
    {
        if (player.Dead) return;

        bool left = Input.GetAxis("Left Action") > 0;
        bool right = Input.GetAxis("Right Action") > 0;

        ActionControlState = left
            ? (right ? ActionControlState.Both : ActionControlState.Left)
            : (right ? ActionControlState.Right : ActionControlState.None);
    }

    // TODO: hook up to menu system.
    private void HandleSceneControls()
    {
        // When the next scene is picked to transition to.
        if (Input.GetKeyDown(KeyCode.Alpha1)) PersistenceManager.Instance.TransitionToNextScene();
        // When the player quits.
        if (Input.GetKeyDown(KeyCode.Escape)) SceneUtils.LoadScene(Scene.GameplayExitScene);
    }
}
