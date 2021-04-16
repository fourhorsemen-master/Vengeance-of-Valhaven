using UnityEngine;
using UnityEngine.VFX;

public class ConsumeObject : StaticAbilityObject
{
    [SerializeField] private VisualEffect visualEffect = null;
    
    public override float StickTime => 5f;

    public static void Create(Vector3 position, Subject onCastSuccessful, Subject onCastFailed)
    {
        ConsumeObject consumeObject = Instantiate(
            AbilityObjectPrefabLookup.Instance.ConsumeObjectPrefab,
            position,
            Quaternion.identity
        );
        onCastSuccessful.Subscribe(consumeObject.OnSuccess);
        onCastFailed.Subscribe(consumeObject.OnFail);
    }

    private void OnSuccess()
    {
        visualEffect.SetBool("Successful", true);
        visualEffect.Stop();
    }

    private void OnFail()
    {
        visualEffect.Stop();
    }
}
