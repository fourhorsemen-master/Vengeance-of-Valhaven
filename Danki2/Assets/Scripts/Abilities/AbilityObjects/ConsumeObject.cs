using UnityEngine;
using UnityEngine.VFX;

public class ConsumeObject : StaticAbilityObject
{
    [SerializeField] private VisualEffect visualEffect = null;
    
    public override float StickTime => 5f;

    public static void Create(Vector3 position, Subject onCastFinished)
    {
        ConsumeObject consumeObject = Instantiate(
            AbilityObjectPrefabLookup.Instance.ConsumeObjectPrefab,
            position,
            Quaternion.identity
        );
        onCastFinished.Subscribe(() => consumeObject.visualEffect.Stop());
    }
}
