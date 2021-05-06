using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Bear : Enemy
{
    [EventRef, SerializeField]
    private string roarEvent = null;

    private bool canRoar = true;
    private float roarDuration;

    public override ActorType Type => ActorType.Bear;

    public Subject CleaveSubject { get; } = new Subject();

    protected override void Start()
    {
        base.Start();

        roarDuration = FmodUtils.GetDuration(roarEvent);

        HealthManager.ModifiedDamageSubject.Subscribe(_ => Roar());
    }

    public void Roar()
    {
        if (!canRoar) return;

        canRoar = false;

        this.WaitAndAct(roarDuration, () => canRoar = true);

        EventInstance fmodEvent = FmodUtils.CreatePositionedInstance(roarEvent, transform.position);
        fmodEvent.start();
        fmodEvent.release();
    }

    public void Swipe()
    {
        InstantCastService.TryCast(
            AbilityReference.Swipe,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
    }

    public void Charge()
    {
        ChannelService.TryStartChannel(AbilityReference.BearCharge);
    }

    public void Maul()
    {
        InstantCastService.TryCast(
            AbilityReference.Maul,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );
    }

    public void Cleave()
    {
        InstantCastService.TryCast(
            AbilityReference.Cleave,
            GetMeleeTargetPosition(transform.position),
            GetMeleeTargetPosition(Centre)
        );

        CleaveSubject.Next();
    }
}
