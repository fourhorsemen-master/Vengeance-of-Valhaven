using System;
using UnityEngine;

public class ForestGolemBoulder : MonoBehaviour
{
    [SerializeField] private TimeTracker timeTracker = null;
    [SerializeField] private AnimationCurve trajectory = null;

    private float animationEndTime;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Action callback;

    public static void Create(ForestGolemBoulder prefab, Vector3 startPosition, Vector3 endPosition, Action callback)
    {
        ForestGolemBoulder forestGolemBoulder = Instantiate(prefab, startPosition, Quaternion.identity);
        forestGolemBoulder.startPosition = startPosition;
        forestGolemBoulder.endPosition = endPosition;
        forestGolemBoulder.callback = callback;
    }

    private void Start()
    {
        animationEndTime = AnimationCurveUtils.GetEndTime(trajectory);
    }

    private void Update()
    {
        if (timeTracker.Time > animationEndTime)
        {
            callback();
            Destroy(gameObject);
            return;
        }

        float animationProportion = timeTracker.Time / animationEndTime;
        Vector3 position = startPosition + (endPosition - startPosition) * animationProportion;
        position.y += trajectory.Evaluate(timeTracker.Time);
        transform.position = position;
    }
}
