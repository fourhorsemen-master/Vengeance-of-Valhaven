public class SceneLoadedManager : Singleton<SceneLoadedManager>
{
    public Subject SceneLoadedSubject { get; } = new Subject();

    private void Start()
    {
        SceneLoadedSubject.Next();
    }
}
