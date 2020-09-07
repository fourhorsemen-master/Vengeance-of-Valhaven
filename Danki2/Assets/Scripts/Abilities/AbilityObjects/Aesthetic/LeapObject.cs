using UnityEngine;

public class LeapObject : StaticAbilityObject
{
    [SerializeField]
    private AudioSource leapSound = null;

    [SerializeField]
    private AudioSource momentumSound = null;

    [SerializeField]
    private GameObject landingVisual = null;

    [SerializeField]
    private GameObject momentumVisual = null;

    public override float StickTime => 2;

    private Transform casterTransform;
    private bool hasMomentum;

    public static LeapObject Create(Transform casterTransform, Subject leapEndSubject, bool hasMomentum)
    {
        LeapObject leapObject = Instantiate(AbilityObjectPrefabLookup.Instance.LeapObjectPrefab, casterTransform.position, casterTransform.rotation);

        leapObject.casterTransform = casterTransform;
        leapObject.hasMomentum = hasMomentum;
        leapObject.Setup(leapEndSubject);

        return leapObject;
    }

    public void PlayMomentumSound()
    {
        leapSound.Stop();
        momentumSound.Play();
    }

    private void Setup(Subject leapEndSubject)
    {
        leapEndSubject.Subscribe(() =>
        {
            gameObject.transform.position = casterTransform.position;
            if (hasMomentum)
            {
                momentumVisual.gameObject.SetActive(true);
                return;
            }
            landingVisual.gameObject.SetActive(true);
        });
    }
}
