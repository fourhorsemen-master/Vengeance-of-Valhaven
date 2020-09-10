using UnityEngine;

public class DisengageObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource landingSound = null;

    [SerializeField]
    private GameObject landingVisual = null;

    public override float StickTime => 2f;

    public static void Create(Transform casterTransform, float duration)
    {
        DisengageObject disengageObject = Instantiate(AbilityObjectPrefabLookup.Instance.DisengageObjectPrefab, casterTransform);

        disengageObject.WaitAndAct(duration, () =>
        {
            disengageObject.Land(casterTransform);
        });
    }

    private void Land(Transform casterTransform)
    {
        transform.position = casterTransform.position;
        landingVisual.gameObject.SetActive(true);
        landingSound.Play();
    }
}
