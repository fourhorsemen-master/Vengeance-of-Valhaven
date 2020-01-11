using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelBar : Bar
{
    Player _player;

    void Awake()
    {
        SetWidth(0f);
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (
            _player.CastingStatus != CastingStatus.ChannelingLeft
            && _player.CastingStatus != CastingStatus.ChannelingRight
        )
        {
            SetWidth(0f);
            return;
        }

        SetWidth(_player.ChannelService.RemainingDuration / _player.ChannelService.TotalDuration);
    }
}
