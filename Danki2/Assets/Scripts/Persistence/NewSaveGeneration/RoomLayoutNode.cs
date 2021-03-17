using System;
using System.Collections.Generic;

public class RoomLayoutNode
{
    public List<RoomLayoutNode> Children { get; } = new List<RoomLayoutNode>();
    public int Id { get; set; } = 0;

    public void IterateDown(Action<RoomLayoutNode> action)
    {
        action(this);
        if (Children.Count == 0) return;
        Children.ForEach(c => c.IterateDown(action));
    }
}
