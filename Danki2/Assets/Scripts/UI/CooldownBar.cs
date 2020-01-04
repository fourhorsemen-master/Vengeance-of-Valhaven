using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{
    Player _player;

    void Awake()
    {
        transform.localScale = new Vector3(0f, 1f, 1f);
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        var maxCooldown = _player.abilityCooldown;
        var remainingCooldown = _player.RemainingAbilityCooldown;
        transform.localScale = new Vector3(remainingCooldown / maxCooldown, 1f, 1f);
    }
}
