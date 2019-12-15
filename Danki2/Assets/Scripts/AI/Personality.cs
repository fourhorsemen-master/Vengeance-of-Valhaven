using System.Collections.Generic;

public class Personality<T> : Dictionary<AIAction, IBehaviour<T>> where T : Actor
{
}
