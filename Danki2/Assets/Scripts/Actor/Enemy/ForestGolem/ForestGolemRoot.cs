using UnityEngine;

public class ForestGolemRoot : MonoBehaviour
{
    [SerializeField] private GameObject root = null;
    [SerializeField] private TimeTracker timeTracker = null;
    [SerializeField] private AnimationCurve height = null;
    [SerializeField] private AnimationCurve rotation = null;

    private void Update()
    {
        Vector3 localPosition = root.transform.localPosition;
        localPosition.y = height.Evaluate(timeTracker.Time);
        root.transform.localPosition = localPosition;
        
        root.transform.Rotate(Vector3.up, rotation.Evaluate(timeTracker.Time));
    }
}
