using UnityEngine;

public class PoisonStabObject : StaticAbilityObject
{
    [SerializeField]
    private ModularPFXComponent mpfx = null;

    public override float StickTime => 5f;

    public static void Create(Transform transform, Subject onFinishMovement)
    {
        Instantiate(
            AbilityObjectPrefabLookup.Instance.PoisonStabObjectPrefab,
            transform
        ).Setup(onFinishMovement);
    }

    private void Setup(Subject onFinishMovement)
    {
        onFinishMovement.Subscribe(() => mpfx.enabled = true);
    }
}
