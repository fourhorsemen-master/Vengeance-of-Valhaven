using System.Collections.Generic;

public class Parser
{
    public List<TemplatedTooltipSegment> Parse(List<Token> tokens)
    {
        List<TemplatedTooltipSegment> templatedTooltipSegments = new List<TemplatedTooltipSegment>();

        for (int i = 0; i < tokens.Count; i++)
        {
            Token token = tokens[i];

            if (token.Type.Equals(TokenType.String))
            {
                templatedTooltipSegments.Add(new TemplatedTooltipSegment(
                    TemplatedTooltipSegmentType.Text,
                    token.Value
                ));
                continue;
            }

            if (token.Type.Equals(TokenType.OpenBrace))
            {
                BindableProperty bindableProperty = BindablePropertyLookup.FromString(tokens[i + 1].Value);
                TemplatedTooltipSegmentType templatedTooltipSegmentType = TemplatedTooltipSegmentTypeLookup.FromBindableProperty(bindableProperty);
                templatedTooltipSegments.Add(new TemplatedTooltipSegment(
                    templatedTooltipSegmentType,
                    null
                ));
                i += 2;
                continue;
            }
        }

        return templatedTooltipSegments;
    }
}
