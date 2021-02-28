using UnityEngine;
using UnityEngine.VFX;

public class ExecuteObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private VisualEffect CastVisualEffect = null;

    [SerializeField]
    private VisualEffect DamageVisualEffect = null;

    public static void Create(Vector3 position, Quaternion rotation, Subject onCastFail, Subject<Vector3> onCastComplete)
    {
        ExecuteObject executeObject = Instantiate(AbilityObjectPrefabLookup.Instance.ExecuteObjectPrefab, position, rotation);

        onCastComplete.Subscribe(location => executeObject.OnCastComplete(location));
        onCastFail.Subscribe(executeObject.OnCastFail);
    }

    private void OnCastComplete(Vector3 location)
    {
        CastVisualEffect.enabled = false;
        this.transform.position = location;
        DamageVisualEffect.enabled = true;
    }

    private void OnCastFail()
    {
        CastVisualEffect.Stop();
    }
}
