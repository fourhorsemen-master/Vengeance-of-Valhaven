public class SingletonExists<T> : StateMachineTrigger where T : Singleton<T>
{
    public override void Activate() {}
    public override void Deactivate() {}
    public override bool Triggers() => Singleton<T>.Exists;
}
