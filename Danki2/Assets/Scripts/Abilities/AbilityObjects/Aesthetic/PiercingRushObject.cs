﻿using UnityEngine;

public class PiercingRushObject : MonoBehaviour
{
    private const float piercingRushDamageObjectDuration = 0.5f;

    [SerializeField]
    private AudioSource jetstreamSound = null;

    [SerializeField]
    private AudioSource piercingRushSound = null;

    [SerializeField]
    private GameObject startMPFXObject = null;

    [SerializeField]
    private GameObject rushMPFXObject = null;

    [SerializeField]
    private GameObject piercingRushDamageObject = null;

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
            NoBonus();
        }
    }

    public void ShowRushDamage(Actor target, Quaternion normalDirection)
    {
        GameObject rushDamageObject = Instantiate(piercingRushDamageObject, target.transform.position, normalDirection);
        this.WaitAndAct(piercingRushDamageObjectDuration, () => Destroy(rushDamageObject));
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
