using Assets.Scripts.KeyMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mortal
{
    public override void Act()
    {
        var bindings = KeyMapper.Mapper.Bindings;

        if (Input.GetKey(bindings.Down))
        {

        }
    }
}
