using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ForestGolemRootIndicator : MonoBehaviour
{
    [SerializeField] private DecalProjector decalProjector = null;
    [SerializeField] private LifetimeTracker lifetimeTracker = null;
    [SerializeField] private AnimationCurve rotationCurve = null;
    [SerializeField] private AnimationCurve scaleCurve = null;
    [SerializeField] private AnimationCurve fadeFactorCurve = null;
    
    private void Update()
    {
        decalProjector.transform.Rotate(Vector3.forward, rotationCurve.Evaluate(lifetimeTracker.Lifetime));

        float scale = scaleCurve.Evaluate(lifetimeTracker.Lifetime);
        decalProjector.size = new Vector3(scale, scale, 1);

        decalProjector.fadeFactor = fadeFactorCurve.Evaluate(lifetimeTracker.Lifetime);
    }
}
