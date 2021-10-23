using UnityEngine;
using UnityEngine.VFX;

public class PoisonStabObject : StaticAbilityObject
{
    [SerializeField] private VisualEffect visualEffect = null;
    [SerializeField] private PoisonStabVisual poisonStabVisualPrefab = null;

    public override float StickTime => 5f;

    public static void Create(Transform transform, Subject onCastFail, Subject onFinishMovement)
    {
        Instantiate(
            AbilityObjectPrefabLookup.Instance.PoisonStabObjectPrefab,
            transform
        ).Setup(onCastFail, onFinishMovement);
    }

    public static void CreateVisual(Vector3 position, Quaternion rotation, Color colour)
    {
        var test = Instantiate(
            AbilityObjectPrefabLookup.Instance.PoisonStabObjectPrefab,
            position,
            rotation
        );
        test.visualEffect.Stop();
        PoisonStabVisual.Create(test.poisonStabVisualPrefab, position, rotation)
            .Mpfx.UpdateEffectColour(colour);
    }

    private void Setup(Subject onCastFail, Subject onFinishMovement)
    {
        onCastFail.Subscribe(() => visualEffect.Stop());
        onFinishMovement.Subscribe(() =>
        {
            visualEffect.Stop();
            PoisonStabVisual.Create(poisonStabVisualPrefab, transform.position, transform.rotation);
        });
    }
}
