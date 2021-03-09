using UnityEngine;
using UnityEngine.VFX;

public class ExecuteObject : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private VisualEffect castVisualEffect = null;

    [SerializeField]
    private VisualEffect damageVisualEffect = null;

    public static void Create(Vector3 position, Quaternion rotation, Subject onCastFail, Subject<Vector3> onCastComplete)
    {
        ExecuteObject executeObject = Instantiate(AbilityObjectPrefabLookup.Instance.ExecuteObjectPrefab, position, rotation);

        onCastComplete.Subscribe(executeObject.OnCastComplete);
        onCastFail.Subscribe(executeObject.OnCastFail);
    }

    private void OnCastComplete(Vector3 location)
    {
        castVisualEffect.Stop();
        transform.position = location;
        damageVisualEffect.enabled = true;
    }

    private void OnCastFail()
    {
        castVisualEffect.Stop();
    }
}
