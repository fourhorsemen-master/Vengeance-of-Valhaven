using UnityEngine;

public class PiercingRushObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource JetstreamSound = null;

    public static PiercingRushObject Create(Transform casterTransform)
    {
        PiercingRushObject prefab = AbilityObjectPrefabLookup.Instance.PiercingRushObjectPrefab;
        PiercingRushObject piercingRushObject = Instantiate(prefab, casterTransform);

        return piercingRushObject;
    }

    public void PlayJetstreamSoundThenDestroy()
    {
        JetstreamSound.Play();
        this.WaitAndAct(JetstreamSound.clip.length, () => Destroy());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
