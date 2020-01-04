using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelBar : MonoBehaviour
{
    Player _player;

    void Awake()
    {
        transform.localScale = new Vector3(0f, 1f, 1f);
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (
            _player.CastingStatus != CastingStatus.ChannelingLeft
            && _player.CastingStatus != CastingStatus.ChannelingRight
        )
        {
            transform.localScale = new Vector3(0f, 1f, 1f);
            return;
        }

        var totalDuration = _player.ChannelService.TotalDuration;
        var remainingDuration = _player.ChannelService.RemainingDuration;
        transform.localScale = new Vector3(remainingDuration / totalDuration, 1f, 1f);
    }
}
