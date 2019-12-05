using UnityEngine;

public class MovementManager : MonoBehaviour
{
    float _moveLockRemainingDuration;
    float _moveLockSpeed;
    bool _moveLockOn;
    Vector3 _moveLockDirection;
    Vector3 _moveDirection;

    void Start()
    {
        _moveLockRemainingDuration = 0f;
        _moveLockSpeed = 0f;
        _moveLockOn = false;
        _moveLockDirection = Vector3.one;
        _moveDirection = Vector3.zero;
    }

    void Update()
    {
        _moveLockRemainingDuration = Mathf.Max(0f, _moveLockRemainingDuration - Time.deltaTime);
        _moveLockOn = _moveLockRemainingDuration > 0 ? _moveLockOn : false;
    }

    private void LateUpdate()
    {
        var speedStat = gameObject.GetComponent<StatsManager>()[Stat.Speed];

        var movementVector = _moveLockOn
            ? Vector3.Normalize(_moveLockDirection) * _moveLockSpeed
            : Vector3.Normalize(_moveDirection) * speedStat;

        transform.Translate(movementVector * Time.deltaTime);
        _moveDirection = Vector3.zero;
    }

    public void LockMovement(float duration, float speed, Vector3 direction)
    {
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
        var vecToMove = target - transform.position;
        MoveAlong(vecToMove);
    }
}
