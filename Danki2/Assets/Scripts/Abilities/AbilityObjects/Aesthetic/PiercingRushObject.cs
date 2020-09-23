using UnityEngine;

public class PiercingRushObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource castSound = null;

    [SerializeField]
    private AudioSource jetstreamSound = null;

    [SerializeField]
    private AudioSource piercingRushSound = null;

    [SerializeField]
    private GameObject startMPFXObject = null;

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

        castSound.Play();

        if (hasJetstream)
        {
            this.WaitAndAct(dashDuration, Jetstream);
        }
        else
        {
            NoBonus();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Jetstream()
    {
        jetstreamSound.Play();
        this.WaitAndAct(jetstreamSound.clip.length, () => Destroy(gameObject));
    }

    private void NoBonus()
    {
        this.WaitAndAct(piercingRushSound.clip.length, () => Destroy(gameObject));
    }
}
