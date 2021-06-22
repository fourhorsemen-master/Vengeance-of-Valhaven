using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ForestGolemRootIndicator : MonoBehaviour
{
    [SerializeField] private DecalProjector decalProjector = null;
    [SerializeField] private LifetimeTracker lifetimeTracker = null;
    [SerializeField] private AnimationCurve rotation = null;
    [SerializeField] private AnimationCurve scale = null;
    [SerializeField] private AnimationCurve fadeFactor = null;
    
    private void Update()
    {
        decalProjector.transform.Rotate(Vector3.forward, rotation.Evaluate(lifetimeTracker.Lifetime));

        float scale = this.scale.Evaluate(lifetimeTracker.Lifetime);
        decalProjector.size = new Vector3(scale, scale, 1);

        decalProjector.fadeFactor = fadeFactor.Evaluate(lifetimeTracker.Lifetime);
    }
}
