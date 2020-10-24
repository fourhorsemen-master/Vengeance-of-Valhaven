using UnityEngine;

public class BashObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource bashSound = null;

    [SerializeField]
    private ModularPFXComponent objectMPFX = null;

    [SerializeField]
    private Color successfulHitColour = Color.white;
    
    public override float StickTime => bashSound.clip.length;

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
