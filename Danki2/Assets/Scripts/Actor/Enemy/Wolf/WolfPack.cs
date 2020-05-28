using System.Collections.Generic;

public class WolfPack
{
    private readonly HashSet<Wolf> members;

    public WolfPack(Wolf wolf1, Wolf wolf2)
    {
        members = new HashSet<Wolf> {wolf1, wolf2};
    }

    public void Add(Wolf wolf)
    {
        members.Add(wolf);
    }

    public void Add(WolfPack pack)
    {
        members.UnionWith(pack.members);
    }
}
