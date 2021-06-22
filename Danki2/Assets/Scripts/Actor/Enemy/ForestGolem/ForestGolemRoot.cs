using UnityEngine;

public class ForestGolemRoot : MonoBehaviour
{
    [SerializeField] private GameObject root = null;
    [SerializeField] private LifetimeTracker lifetimeTracker = null;
    [SerializeField] private AnimationCurve height = null;
    [SerializeField] private AnimationCurve rotation = null;

    private void Start()
    {
        Rotate(Random.Range(0, 360));
    }

    private void Update()
    {
        Vector3 localPosition = root.transform.localPosition;
        localPosition.y = height.Evaluate(lifetimeTracker.Lifetime);
        root.transform.localPosition = localPosition;
        
        Rotate(rotation.Evaluate(lifetimeTracker.Lifetime));
    }

    private void Rotate(float angle) => root.transform.Rotate(Vector3.up, angle);
}
