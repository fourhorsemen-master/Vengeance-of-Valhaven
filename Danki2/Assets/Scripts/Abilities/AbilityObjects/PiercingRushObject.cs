using UnityEngine;

public class PiercingRushObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource jetstreamSound = null;

    [SerializeField]
    private AudioSource piercingRushSound = null;

    [SerializeField]
    private GameObject startMPFXObject = null;

    [SerializeField]
    private GameObject rushMPFXObject = null;

    [SerializeField]
    private GameObject landMPFXObject = null;

    public override float StickTime =>  5f;

    public static void Create(Transform transform, Subject onCastCancelled, Subject<float> onCastComplete, bool hasJetStream)
    {
        PiercingRushObject piercingRushObject = Instantiate(AbilityObjectPrefabLookup.Instance.PiercingRushObjectPrefab, transform);

        onCastCancelled.Subscribe(piercingRushObject.Destroy);
        onCastComplete.Subscribe(duration => piercingRushObject.OnCastComplete(hasJetStream, duration));
    }

    private void OnCastComplete(bool hasJetstream, float dashDuration)
    {
        startMPFXObject.SetActive(false);
        rushMPFXObject.SetActive(true);

        piercingRushSound.Play();
        
        this.WaitAndAct(dashDuration, () => Landing(hasJetstream));
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Landing(bool hasJetstream)
    {
        rushMPFXObject.SetActive(false);
        landMPFXObject.SetActive(true);
        if (hasJetstream) jetstreamSound.Play();
    }
}
