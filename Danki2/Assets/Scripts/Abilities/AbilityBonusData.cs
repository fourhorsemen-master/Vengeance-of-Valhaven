public class AbilityBonusData
{
    public string DisplayName { get; }
    public string Tooltip { get; }
    public OrbCollection RequiredOrbs { get; }

    public AbilityBonusData(string displayName, string tooltip, OrbCollection requiredOrbs)
    {
        DisplayName = displayName;
        Tooltip = tooltip;
        RequiredOrbs = requiredOrbs;
    }
}
