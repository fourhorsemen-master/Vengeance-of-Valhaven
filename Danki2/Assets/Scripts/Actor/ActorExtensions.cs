public static class ActorExtensions
{
    private static string playerTag = "Player";
    private static string enemyTag = "Enemy";
    private static string friendlyTag = "Friendly";

    public static bool IsPlayer(this Actor actor)
    {
        return actor.tag == playerTag;
    }

    public static bool IsEnemy(this Actor actor)
    {
        return actor.tag == enemyTag;
    }

    public static bool IsFriendly(this Actor actor)
    {
        return actor.tag == friendlyTag;
    }

    public static bool Opposes(this Actor actor, Actor target)
    {
        return actor.IsEnemy() ^ target.IsEnemy();
    }
}
