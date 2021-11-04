using UnityEngine;

public class PoisonStabVisual : StaticAbilityObject
{
    public override float StickTime => 5f;

    [SerializeField]
    private ModularPFXComponent mpfx;

    public static PoisonStabVisual Create(Vector3 position, Quaternion rotation)
    {
        return Instantiate(
            AbilityObjectPrefabLookup.Instance.PoisonStabVisualPrefab,
            position,
            rotation
        );
    }

    public void SetColour(Color colour) => mpfx.UpdateEffectColour(colour);
}
