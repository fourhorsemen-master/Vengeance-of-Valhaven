public class TooltipSegment
{
    public TooltipSegmentType Type { get; }
    public string Value { get; }

    public TooltipSegment(TooltipSegmentType type, string value)
    {
        Type = type;
        Value = value;
    }
}
