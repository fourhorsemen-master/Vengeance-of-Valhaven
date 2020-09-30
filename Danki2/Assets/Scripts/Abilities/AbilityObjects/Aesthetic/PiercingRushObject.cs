using UnityEngine;

public class PiercingRushObject : MonoBehaviour
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

    public static PiercingRushObject Create(Transform transform, Subject onCastCancelled)
    {
        PiercingRushObject piercingRushObject = Instantiate(AbilityObjectPrefabLookup.Instance.PiercingRushObjectPrefab, transform);

        piercingRushObject.startMPFXObject.SetActive(true);

        onCastCancelled.Subscribe(piercingRushObject.Destroy);

        return piercingRushObject;
    }

    public void OnEnd(bool hasJetstream, float dashDuration)
    {
        startMPFXObject.SetActive(false);
        rushMPFXObject.SetActive(true);

        piercingRushSound.Play();

        if (hasJetstream)
        {
            this.WaitAndAct(dashDuration, Jetstream);
        }
        else
        {
            this.WaitAndAct(dashDuration, NoBonus);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Jetstream()
    {
        rushMPFXObject.SetActive(false);
        landMPFXObject.SetActive(true);
        jetstreamSound.Play();
        this.WaitAndAct(jetstreamSound.clip.length, () => Destroy(gameObject));
    }

    private void NoBonus()
    {
        rushMPFXObject.SetActive(false);
        landMPFXObject.SetActive(true);
        this.WaitAndAct(piercingRushSound.clip.length, () => Destroy(gameObject));
    }
}
