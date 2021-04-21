using System;
using UnityEngine;

public class GuidedOrbObject : MonoBehaviour
{
    private float requiredExplosionDistance;
    private float speed;
    private float rotationSpeed;
    private Transform target;
    private Action<Vector3> explosionCallback;
    
    public static void Fire(
        float maxDuration,
        float requiredExplosionDistance,
        float speed,
        float rotationSpeed,
        Transform target,
        Vector3 position,
        Action<Vector3> explosionCallback
    )
    {
        GuidedOrbObject guidedOrbObject = Instantiate(
            AbilityObjectPrefabLookup.Instance.GuidedOrbObjectPrefab,
            position,
            Quaternion.LookRotation(target.position - position)
        );
        guidedOrbObject.requiredExplosionDistance = requiredExplosionDistance;
        guidedOrbObject.speed = speed;
        guidedOrbObject.rotationSpeed = rotationSpeed;
        guidedOrbObject.target = target;
        guidedOrbObject.explosionCallback = explosionCallback;

        guidedOrbObject.WaitAndAct(maxDuration, guidedOrbObject.Explode);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= requiredExplosionDistance)
        {
            Explode();
            return;
        }

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(
            transform.forward,
            target.position - transform.position,
            rotationSpeed * Time.deltaTime,
            Mathf.Infinity
        ));
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    private void Explode()
    {
        explosionCallback(transform.position);
        Destroy(gameObject);
    }
}
