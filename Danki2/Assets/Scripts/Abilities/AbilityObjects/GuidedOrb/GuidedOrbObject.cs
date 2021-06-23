using System;
using UnityEngine;

public class GuidedOrbObject : MonoBehaviour
{
    [SerializeField]
    private GuidedOrbExplosion guidedOrbExplosionPrefab = null;
    
    private float speed;
    private float rotationSpeed;
    private Transform target;
    private Action<Vector3> explosionCallback;
    private Subscription casterDeathSubscription;

    public static void Fire(
        float maxDuration,
        float speed,
        float rotationSpeed,
        Transform target,
        Vector3 position,
        Action<Vector3> explosionCallback,
        Subject casterDeathSubject
    )
    {
        GuidedOrbObject guidedOrbObject = Instantiate(
            AbilityObjectPrefabLookup.Instance.GuidedOrbObjectPrefab,
            position,
            Quaternion.LookRotation(target.position - position)
        );

        guidedOrbObject.speed = speed;
        guidedOrbObject.rotationSpeed = rotationSpeed;
        guidedOrbObject.target = target;
        guidedOrbObject.explosionCallback = explosionCallback;
        guidedOrbObject.casterDeathSubscription = casterDeathSubject.Subscribe(() => Destroy(guidedOrbObject.gameObject));

        guidedOrbObject.WaitAndAct(maxDuration, guidedOrbObject.Explode);
    }

    private void OnTriggerEnter(Collider other) => Explode();

    private void Update()
    {
        UpdateRotation();
        UpdatePosition();
    }

    private void UpdateRotation()
    {
        Vector3 newRotation = Vector3.RotateTowards(
            transform.forward,
            target.position - transform.position,
            rotationSpeed * Time.deltaTime,
            Mathf.Infinity
        );

        newRotation.y = 0;

        transform.rotation = Quaternion.LookRotation(newRotation);
    }

    private void UpdatePosition()
    {
        var newPosition = transform.position + speed * Time.deltaTime * transform.forward;

        newPosition.y = Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime);

        transform.position = newPosition;
    }

    private void Explode()
    {
        GuidedOrbExplosion.Create(guidedOrbExplosionPrefab, transform.position);
        explosionCallback(transform.position);
        casterDeathSubscription.Unsubscribe();
        Destroy(gameObject);
    }
}
