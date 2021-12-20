using UnityEngine;

public class PoisonStabVisual : AbilityObject
{
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
