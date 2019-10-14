using Assets.Scripts.KeyMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mortal
{
    public override void Act()
    {
        var upBinding = KeyMapper.Mapper.GetBinding(Control.Up);
        var downBinding = KeyMapper.Mapper.GetBinding(Control.Down);
        var leftBinding = KeyMapper.Mapper.GetBinding(Control.Left);
        var rightBinding = KeyMapper.Mapper.GetBinding(Control.Right);
        var leftActionBinding = KeyMapper.Mapper.GetBinding(Control.LeftAction);
        var rightActionBinding = KeyMapper.Mapper.GetBinding(Control.RightAction);
        var dashBinding = KeyMapper.Mapper.GetBinding(Control.Dash);

        if (Input.GetKey(upBinding))
        {

        }
    }
}
