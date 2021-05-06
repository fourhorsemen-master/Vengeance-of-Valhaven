using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Bear : Enemy
{
    [EventRef, SerializeField]
    private string roarEvent = null;

    public override ActorType Type => ActorType.Bear;

    public Subject CleaveSubject { get; } = new Subject();

    protected override void Start()
    {
        base.Start();

        HealthManager.ModifiedDamageSubject.Subscribe(_ => Roar());
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

    private void Roar()
    {
        EventInstance fmodEvent = FmodUtils.CreatePositionedInstance(roarEvent, transform.position);
        fmodEvent.start();
        fmodEvent.release();
    }
}
