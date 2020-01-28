public static class ActorExtensions
{
    public static bool Opposes(this Actor actor, Actor target)
    {
        return actor.tag != target.tag;
    }
}
