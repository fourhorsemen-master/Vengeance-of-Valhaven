using UnityEngine;

public class BashObject : StaticAbilityObject
{
    [SerializeField]
    private ModularPFXComponent objectMPFX = null;

    [SerializeField]
    private Color successfulHitColour = Color.white;
    
    public override float StickTime => 5f;

    public static void Create(Vector3 position, bool successfulHit)
    {
        BashObject bashObject = Instantiate(AbilityObjectPrefabLookup.Instance.BashObjectPrefab, position, Quaternion.identity);

        if (successfulHit) bashObject.SuccessfulHit();
    }

    private void SuccessfulHit()
    {
        objectMPFX.UpdateEffectColour(successfulHitColour);
    }
}
