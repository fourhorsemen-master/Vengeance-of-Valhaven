public class FmodParameter
{
    public string Name { get; private set; }
    public float Value { get; private set; }

    public FmodParameter(string name, float value)
    {
        Name = name;
        Value = value;
    }
}
