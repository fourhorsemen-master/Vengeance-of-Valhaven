using UnityEngine;

public class LungeObject : StaticAbilityObject
{
    [SerializeField]
    private ModularPFXComponent modularPFX = null;

    public override float StickTime => 2f;

    public static LungeObject Create(Vector3 position, Quaternion rotation, Subject<Vector3> onFinishMovement)
    {
        LungeObject prefab = AbilityObjectPrefabLookup.Instance.LungeObjectPrefab;
        LungeObject lungeObject = Instantiate(prefab, position, rotation);
        lungeObject.Setup(onFinishMovement);

        return lungeObject;
    }

    private void Setup(Subject<Vector3> onFinishMovement)
    {
        onFinishMovement.Subscribe(position =>
        {
            transform.position = position;
            modularPFX.enabled = true;
        });
    }
}

