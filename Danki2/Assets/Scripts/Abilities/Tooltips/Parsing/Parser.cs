using System.Collections.Generic;

public class Parser
{
    /// <summary>
    /// Parses a list of tokens into a list of templated tooltip segments. This list is a representation of the
    /// parts of the tooltip that can be used to bind in the explicit values required for rendering.
    ///
    /// NOTE: This assumes that the tokens passed in have been validated to have the correct syntax.
    /// </summary>
    /// <param name="tokens"> The tokens to parse. </param>
    /// <returns> The list of templated tooltip segments. </returns>
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

                if (BindablePropertyLookup.RequiresArgument(bindableProperty))
                {
                    templatedTooltipSegments.Add(new TemplatedTooltipSegment(
                        templatedTooltipSegmentType,
                        tokens[i + 3].Value
                    ));
                    i += 4;
                    continue;
                }
                else
                {
                    templatedTooltipSegments.Add(new TemplatedTooltipSegment(
                        templatedTooltipSegmentType,
                        null
                    ));
                    i += 2;
                    continue;
                }
            }
        }

        return templatedTooltipSegments;
    }
}
