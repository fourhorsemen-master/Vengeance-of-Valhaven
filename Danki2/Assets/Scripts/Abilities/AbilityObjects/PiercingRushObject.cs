using UnityEngine;

public class PiercingRushObject : StaticAbilityObject
{
    [SerializeField]
    private GameObject startMPFXObject = null;

    [SerializeField]
    private GameObject rushMPFXObject = null;

    [SerializeField]
    private GameObject landMPFXObject = null;

    public override float StickTime =>  5f;

    public static void Create(Transform transform, Subject onCastCancelled, Subject<float> onCastComplete)
    {
        PiercingRushObject piercingRushObject = Instantiate(AbilityObjectPrefabLookup.Instance.PiercingRushObjectPrefab, transform);

        onCastCancelled.Subscribe(piercingRushObject.Destroy);
        onCastComplete.Subscribe(duration => piercingRushObject.OnCastComplete(duration));
    }

    private void OnCastComplete(float dashDuration)
    {
        startMPFXObject.SetActive(false);
        rushMPFXObject.SetActive(true);
        
        this.WaitAndAct(dashDuration, () => Landing());
    }

    private void Destroy() => Destroy(gameObject);

    private void Landing()
    {
        rushMPFXObject.SetActive(false);
        landMPFXObject.SetActive(true);
    }
}
