using UnityEngine;

public class LeapObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource landingSound = null;

    [SerializeField]
    private AudioSource hitSound = null;

    [SerializeField]
    private GameObject landingVisual = null;

    public override float StickTime => 2;

    private Transform casterTransform;
    private bool hasMomentum;

    public static LeapObject Create(Transform casterTransform, Subject leapEndSubject, bool hasMomentum)
    {
        LeapObject leapObject = Instantiate(AbilityObjectPrefabLookup.Instance.LeapObjectPrefab, casterTransform.position, Quaternion.identity);

        leapObject.casterTransform = casterTransform;
        leapObject.hasMomentum = hasMomentum;
        leapObject.Setup(leapEndSubject);

        return leapObject;
    }

    public void PlayHitSound()
    {
        hitSound.Play();
    }

    private void Setup(Subject leapEndSubject)
    {
        leapEndSubject.Subscribe(() =>
        {
            landingSound.Play();

            Vector3 position = casterTransform.position;

            if (hasMomentum)
            {
                SmashObject.Create(position, false);
                return;
            }

            gameObject.transform.position = position;
            landingVisual.gameObject.SetActive(true);
        });
    }
}
