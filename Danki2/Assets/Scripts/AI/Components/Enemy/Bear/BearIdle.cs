using UnityEngine;

public class BearIdle : IStateMachineComponent
{
    private readonly Bear bear;
    private readonly float minIdleSoundTimer;
    private readonly float maxIdleSoundTimer;
    private float idleSoundTimer;

    public BearIdle(Bear bear, float minIdleSoundTimer, float maxIdleSoundTimer)
    {
        this.bear = bear;
        this.minIdleSoundTimer = minIdleSoundTimer;
        this.maxIdleSoundTimer = maxIdleSoundTimer;
    }

    public void Enter() => ResetIdleSoundTimer();

    public void Exit() {}

    public void Update()
    {
        idleSoundTimer -= Time.deltaTime;

        if (idleSoundTimer <= 0f)
        {
            bear.PlayIdleSound();
            ResetIdleSoundTimer();
        }
    }

    private void ResetIdleSoundTimer()
    {
        idleSoundTimer = Random.Range(minIdleSoundTimer, maxIdleSoundTimer);
    }
}
