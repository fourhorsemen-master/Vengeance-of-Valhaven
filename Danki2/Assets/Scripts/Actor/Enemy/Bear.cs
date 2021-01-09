using UnityEngine;

public class Bear : Enemy
{
    public override ActorType Type => ActorType.Bear;

    protected override void Start()
    {
        base.Start();

        ChannelService.Target = RoomManager.Instance.Player;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.K)) Charge();
        if (Input.GetKeyDown(KeyCode.L)) Stop();
    }

    private void Charge() => ChannelService.TryStartChannel(AbilityReference.BearCharge);
    private void Stop() => ChannelService.CancelChannel();
}
