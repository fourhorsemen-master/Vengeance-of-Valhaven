using UnityEngine;
using UnityEngine.VFX;

public class ExecuteObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private VisualEffect castVisualEffect = null;

    [SerializeField]
    private ExecuteVisual executeVisualPrefab = null;

    public static void Create(Vector3 position, Quaternion rotation, Subject onCastFail, Subject<Actor> onCastComplete)
    {
        ExecuteObject executeObject = Instantiate(AbilityObjectPrefabLookup.Instance.ExecuteObjectPrefab, position, rotation);

        onCastComplete.Subscribe(executeObject.OnCastComplete);
        onCastFail.Subscribe(executeObject.OnCastFail);
    }

    private void OnCastComplete(Actor target)
    {
        castVisualEffect.Stop();
        ExecuteVisual.Create(executeVisualPrefab, target.transform);
    }

    private void OnCastFail()
    {
        castVisualEffect.Stop();
    }
}
