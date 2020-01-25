using UnityEngine;

public class MovementManager
{
    const float rotationSmoothing = 0.1f;

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
        if (_moveLockRemainingDuration <= 0)
        {
            _moveLockOn = false;
            _actor.gameObject.SetLayer(Layer.Default);
        }

        var speedStat = _actor.GetStat(Stat.Speed);

        var movementVector = _moveLockOn
            ? Vector3.Normalize(_moveLockDirection) * _moveLockSpeed
            : Vector3.Normalize(_moveDirection) * speedStat;

        _rigidbody.MovePosition(_rigidbody.position + movementVector * Time.deltaTime);

        RotateForwards(movementVector);

        _moveDirection = Vector3.zero;
    }

    public void LockMovement(float duration, float speed, Vector3 direction, bool @override = false, bool passThrough = false)
    {
        if (_moveLockOn && !@override) return;
        if (passThrough)
        {
            _actor.gameObject.SetLayer(Layer.Ethereal);
        }

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

    private void RotateForwards(Vector3 movementVector)
    {
        if (!movementVector.Equals(Vector3.zero))
        {
            var desiredRotation = Quaternion.LookRotation(movementVector);
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, desiredRotation, rotationSmoothing);
        }
    }
}