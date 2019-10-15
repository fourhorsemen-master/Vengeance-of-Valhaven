using Assets.Scripts.AI;
using Assets.Scripts.KeyMapping;
using UnityEngine;

public class Player : Mortal
{
    public override AI AI => null;

    public override void Act()
    {
        var bindings = KeyMapper.Mapper.Bindings;

        // Move etc. according to bindings
    }
}
