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
    private Vector3 _nextRotation;

    public MovementManager(Actor actor, Rigidbody rigidbody)
    {
        _actor = actor;
        _rigidbody = rigidbody;
        _moveLockRemainingDuration = 0f;
        _moveLockSpeed = 0f;
        _moveLockOn = false;
        _moveLockDirection = Vector3.one;
        _moveDirection = Vector3.zero;
        _nextRotation = Vector3.zero;
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

        int speedStat = _actor.GetStat(Stat.Speed);

        Vector3 movementVector = _moveLockOn
            ? Vector3.Normalize(_moveLockDirection) * _moveLockSpeed
            : Vector3.Normalize(_moveDirection) * speedStat;

        _rigidbody.MovePosition(_rigidbody.position + movementVector * Time.deltaTime);

        if (_nextRotation == Vector3.zero || (_moveLockOn && movementVector.magnitude > 0))
        {
            RotateTorwards(movementVector);
        }
        else
        {
            RotateTorwards(_nextRotation);
        }

        _nextRotation = Vector3.zero;
        _moveDirection = Vector3.zero;
    }

    public void LockMovement(float duration, float speed, Vector3 direction, bool rotateForwards = true, bool @override = false, bool passThrough = false)
    {
        if (_moveLockOn && !@override) return;
        if (passThrough)
        {
            _actor.gameObject.SetLayer(Layer.Ethereal);
        }

        if (rotateForwards)
        {
            _rigidbody.rotation = Quaternion.LookRotation(direction);
        }

        _moveLockRemainingDuration = duration;
        _moveLockSpeed = speed;
        _moveLockDirection = direction;
        _moveLockOn = true;
    }

    public void Root(float duration, Vector3 faceDirection, bool @override = false)
    {
        LockMovement(duration, 0f, faceDirection, true, @override);
    }

    public void MoveAlong(Vector3 vec)
    {
        _moveDirection = vec;
    }

    public void MoveToward(Vector3 target)
    {
        Vector3 vecToMove = target - _rigidbody.position;
        MoveAlong(vecToMove);
    }

    public void FixNextRotation(Vector3 direction)
    {
        _nextRotation = direction;
    }

    private void RotateTorwards(Vector3 direction)
    {
        if (!direction.Equals(Vector3.zero))
        {
            Quaternion desiredRotation = Quaternion.LookRotation(direction);
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, desiredRotation, rotationSmoothing);
        }
    }
}