using System.Collections.Generic;
using UnityEngine;

public static class ParsedTooltipLookup
{
    private static readonly Lexer lexer = new Lexer();
    private static readonly TokenValidator validator = new TokenValidator();
    private static readonly Parser parser = new Parser();

    private static readonly Dictionary<AbilityReference, List<TemplatedTooltipSegment>> lookup =
        new Dictionary<AbilityReference, List<TemplatedTooltipSegment>>();

    /// <summary>
    /// Gets the list of templated tooltip segments for the given ability reference. These values are cached, as once
    /// a tooltip has been parsed, its templated tooltip segments won't change again.
    /// </summary>
    /// <param name="abilityReference"> The ability to get the segments for </param>
    /// <returns> The list of templated tooltip segments </returns>
    public static List<TemplatedTooltipSegment> GetTemplatedTooltipSegments(AbilityReference abilityReference)
    {
        if (lookup.TryGetValue(abilityReference, out List<TemplatedTooltipSegment> templatedTooltipSegments))
        {
            return templatedTooltipSegments;
        }

        templatedTooltipSegments = GenerateTemplatedTooltipSegments(abilityReference);
        lookup[abilityReference] = templatedTooltipSegments;
        return templatedTooltipSegments;
    }

    private static List<TemplatedTooltipSegment> GenerateTemplatedTooltipSegments(AbilityReference abilityReference)
    {
        string tooltip = AbilityMetadataLookup.Instance.GetAbilityTooltip(abilityReference);
        List<Token> tokens = lexer.Lex(tooltip);

        if (!validator.HasValidSyntax(tokens))
        {
            Debug.LogError($"Tooltip does not have valid syntax, value was: \"{tooltip}\"");
            return new List<TemplatedTooltipSegment>();
        }

        return parser.Parse(tokens);
    }
}
