using UnityEngine;

public class PiercingRushObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource jetstreamSound = null;

    [SerializeField]
    private AudioSource piercingRushSound = null;

    public static PiercingRushObject Create(Transform casterTransform, bool hasJetstream, float dashDuration)
    {
        PiercingRushObject prefab = AbilityObjectPrefabLookup.Instance.PiercingRushObjectPrefab;
        PiercingRushObject piercingRushObject = Instantiate(prefab, casterTransform);

        if (hasJetstream)
        {
            piercingRushObject.WaitAndAct(dashDuration, piercingRushObject.Jetstream);
        }
        else
        {
            piercingRushObject.NoBonus();
        }

        return piercingRushObject;
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
