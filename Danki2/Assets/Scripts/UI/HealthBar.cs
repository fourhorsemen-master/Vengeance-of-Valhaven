using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // TODO: Change healthbar to show player health rather than terget dummy health.
    TargetDummy _player;

    void Awake()
    {
        transform.localScale = new Vector3(0f, 1f, 1f);
        _player = FindObjectOfType<TargetDummy>();
    }

    void Update()
    {
        var totalHealth = _player.GetStat(Stat.MaxHealth);
        var remainingHealth = Mathf.Max(_player.Health, 0f);
        transform.localScale = new Vector3(remainingHealth / totalHealth, 1f, 1f);
    }
}
