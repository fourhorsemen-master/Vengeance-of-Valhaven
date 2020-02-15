using UnityEngine;

public class HealthScreenBar : ScreenBar
{
    [SerializeField]
    private Player _player = null;

    void Update()
    {
        SetWidth(Mathf.Max(_player.Health, 0f) / _player.GetStat(Stat.MaxHealth));
    }
}
