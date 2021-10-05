using System;
using UnityEngine;

public class ForestGolemBoulder : MonoBehaviour
{
    [SerializeField] private LifetimeTracker lifetimeTracker = null;
    [SerializeField] private AnimationCurve trajectoryCurve = null;
    [SerializeField] private float smashScaleFactor = 0;
    [SerializeField] private float smashSpikeScaleFactor = 0;

    private float animationEndTime;
    private float spikeDamageInitialDelay;
    private float spikeDamageInterval;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Action callback;
    private Action spikeDamageCallback;

    private bool finished = false;

    public static void Create(
        ForestGolemBoulder prefab,
        Vector3 startPosition,
        Vector3 endPosition,
        Action OnLand,
        Action landed,
        float spikeDamageInitialDelay,
        float spikeDamageInterval
    )
    {
        ForestGolemBoulder forestGolemBoulder = Instantiate(prefab, startPosition, Quaternion.identity);
        forestGolemBoulder.startPosition = startPosition;
        forestGolemBoulder.endPosition = endPosition;
        forestGolemBoulder.callback = OnLand;
        forestGolemBoulder.spikeDamageCallback = landed;
        forestGolemBoulder.spikeDamageInitialDelay = spikeDamageInitialDelay;
        forestGolemBoulder.spikeDamageInterval = spikeDamageInterval;
    }

    private void Start()
    {
        animationEndTime = AnimationCurveUtils.GetEndTime(trajectoryCurve);
    }

    private void Update()
    {
        if (finished) return;

        if (lifetimeTracker.Lifetime > animationEndTime) Land();
        else MoveAlongTrajectory();
    }

    private void Land()
    {
        transform.position = endPosition;
        SmashObject.Create(endPosition, smashScaleFactor);
        callback();
        finished = true;
        this.ActOnInterval(
            spikeDamageInterval,
            _ =>
            {
                spikeDamageCallback();
                SmashObject.Create(endPosition, smashSpikeScaleFactor);
            },
            spikeDamageInitialDelay
        );
    }

    private void MoveAlongTrajectory()
    {
        float animationProportion = lifetimeTracker.Lifetime / animationEndTime;
        Vector3 position = startPosition + (endPosition - startPosition) * animationProportion;
        position.y += trajectoryCurve.Evaluate(lifetimeTracker.Lifetime);
        transform.position = position;
    }
}
