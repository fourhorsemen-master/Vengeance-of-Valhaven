using UnityEngine;

public class MovementManager
{
    private Actor _actor;
    private Rigidbody _rigidbody;
    private float _moveLockRemainingDuration;
    private float _moveLockSpeed;
    private bool _moveLockOn;
    private Vector3 _moveLockDirection;
    private Vector3 _moveDirection;

    public MovementManager(Actor actor, Rigidbody rigidbody)
    {
        _actor = actor;
        _rigidbody = rigidbody;
        _moveLockRemainingDuration = 0f;
        _moveLockSpeed = 0f;
        _moveLockOn = false;
        _moveLockDirection = Vector3.one;
        _moveDirection = Vector3.zero;
    }

    /// <summary>
    /// Called by the Actor component during orchestration.
    /// </summary>
    public void ExecuteMovement()
    {
        _moveLockRemainingDuration = Mathf.Max(0f, _moveLockRemainingDuration - Time.deltaTime);
        _moveLockOn = _moveLockRemainingDuration > 0 ? _moveLockOn : false;

        var speedStat = _actor.GetStat(Stat.Speed);

        var movementVector = _moveLockOn
            ? Vector3.Normalize(_moveLockDirection) * _moveLockSpeed
            : Vector3.Normalize(_moveDirection) * speedStat;

        _rigidbody.MovePosition(_rigidbody.position + movementVector * Time.deltaTime);
        if (!movementVector.Equals(Vector3.zero))
        {
            _rigidbody.MoveRotation(Quaternion.LookRotation(movementVector));
        }

        _moveDirection = Vector3.zero;
    }

    public void LockMovement(float duration, float speed, Vector3 direction, bool @override = false)
    {
        if (_moveLockOn && !@override) return;

        _moveLockRemainingDuration = duration;
        _moveLockSpeed = speed;
        _moveLockDirection = direction;
        _moveLockOn = true;
    }

    public void MoveAlong(Vector3 vec)
    {
        _moveDirection = vec;
    }

    public void MoveToward(Vector3 target)
    {
        var vecToMove = target - _rigidbody.position;
        MoveAlong(vecToMove);
    }
}