using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ForestGolemRootIndicator : MonoBehaviour
{
    [SerializeField] private DecalProjector decalProjector = null;
    [SerializeField] private TimeTracker timeTracker = null;
    [SerializeField] private AnimationCurve rotation = null;
    [SerializeField] private AnimationCurve scale = null;
    [SerializeField] private AnimationCurve fadeFactor = null;
    
    private void Update()
    {
        decalProjector.transform.Rotate(Vector3.forward, rotation.Evaluate(timeTracker.Time));

        float scale = this.scale.Evaluate(timeTracker.Time);
        decalProjector.size = new Vector3(scale, scale, 1);

        decalProjector.fadeFactor = fadeFactor.Evaluate(timeTracker.Time);
    }
}
