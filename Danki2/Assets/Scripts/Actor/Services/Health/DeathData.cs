using System.Collections.Generic;

public class DeathData
{
    public DeathData(List<Empowerment> empowerments = null)
    {
        Empowerments = empowerments ?? new List<Empowerment>();
    }

    public List<Empowerment> Empowerments { get; set; }
}
