using UnityEngine;

public class BandageObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource bandageSound = null;

    public static BandageObject Create(Transform casterTransform)
    {
        return Instantiate(AbilityObjectPrefabLookup.Instance.BandageObjectPrefab, casterTransform);
    }

    public void PlaySound()
    {
        bandageSound.Play();
    }

    public void Destroy()
    {
        this.WaitAndAct(bandageSound.clip.length, () => Destroy(gameObject));
    }
}
