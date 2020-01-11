using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Bar
{
    Player _player;

    void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        SetWidth(Mathf.Max(_player.Health, 0f) / _player.GetStat(Stat.MaxHealth));
    }
}
