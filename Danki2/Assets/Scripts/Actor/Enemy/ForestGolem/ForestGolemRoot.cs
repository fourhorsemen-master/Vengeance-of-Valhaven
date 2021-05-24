using UnityEngine;

public class ForestGolemRoot : MonoBehaviour
{
    [SerializeField] private GameObject root = null;
    [SerializeField] private TimeTracker timeTracker = null;
    [SerializeField] private AnimationCurve height = null;
    [SerializeField] private AnimationCurve rotation = null;

    private void Update()
    {
        Vector3 position = root.transform.position;
        position.y = height.Evaluate(timeTracker.Time);
        root.transform.position = position;
        
        root.transform.Rotate(Vector3.up, rotation.Evaluate(timeTracker.Time));
    }
}
