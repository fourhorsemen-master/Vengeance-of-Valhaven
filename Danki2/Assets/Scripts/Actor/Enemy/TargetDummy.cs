public class TargetDummy : Enemy
{
    public override ActorType Type => ActorType.TargetDummy;

    protected override void Start()
    {
        base.Start();

        MovementManager.LookAt(CustomCamera.Instance.transform.position);
    }
}
