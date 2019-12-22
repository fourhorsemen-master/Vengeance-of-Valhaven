using System.Collections.Generic;

public class Personality<T> : Dictionary<AIAction, Behaviour<T>> where T : Actor
{
}
