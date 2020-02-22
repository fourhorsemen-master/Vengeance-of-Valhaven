﻿using System.Collections;
using UnityEngine;

public abstract class StaticObject : MonoBehaviour
{
    protected void InitialiseAbility(float stickTime)
    {
        StartCoroutine(DissapearAfter(stickTime));
    }

    private IEnumerator DissapearAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
