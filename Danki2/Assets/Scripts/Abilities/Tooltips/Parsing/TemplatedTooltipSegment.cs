public class TemplatedTooltipSegment
{
    public TemplatedTooltipSegmentType Type { get; }
    public string Value { get; }

    public TemplatedTooltipSegment(TemplatedTooltipSegmentType type, string value)
    {
        Type = type;
        Value = value;
    }
}
