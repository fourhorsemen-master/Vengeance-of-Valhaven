using Assets.Scripts.KeyMapping;
using UnityEngine;

public class Player : Mortal
{

    [SerializeField, Range(1f, 100f), Tooltip("Recommended value: 16.")]
    private float moveSpeed = 1f; //This will be inherited in the future. Keeping here for now as to avoid changing other files.

    private float moveSpeedMultiplier = 100f; //Allows for nice moveSpeed values in the inspector.
    private Vector3 vecToMove = new Vector3();
    private KeyBindings keyBinds;

    public override void Act()
    {
        Move();
    }

    private void Move()
    {
        if (keyBinds == null) keyBinds = KeyMapper.Mapper.Bindings;

        vecToMove = Vector3.zero;

        if (Input.GetKey(keyBinds.Right)) vecToMove.x += 1f;
        if (Input.GetKey(keyBinds.Left)) vecToMove.x -= 1f;
        if (Input.GetKey(keyBinds.Up)) vecToMove.y += 1f;
        if (Input.GetKey(keyBinds.Down)) vecToMove.y -= 1f;

        //Handles diagonal speed reduction.
        vecToMove.Normalize();
        vecToMove *= moveSpeed/moveSpeedMultiplier;

        transform.Translate(vecToMove);
    }
}
