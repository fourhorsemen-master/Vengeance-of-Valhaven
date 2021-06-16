﻿using System.Collections.Generic;

public class TokenValidator
{
    /// <summary>
    /// Validates that a list of given tokens has valid syntax. This is so that the parser can make certain
    /// assumptions about the tokens. For example, that an open brace "{" is followed by a valid bindable
    /// property and then followed by a closing brace "}", or a pipe "|", then a valid argument, then a
    /// closing brace "}".
    /// </summary>
    /// <param name="tokens"> The tokens to validate </param>
    /// <returns> Whether the given tokens have valid syntax. </returns>
    public bool HasValidSyntax(List<Token> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            Token token = tokens[i];
            
            if (token.Type.Equals(TokenType.String)) continue;

            if (token.Type.Equals(TokenType.OpenBrace))
            {
                if (i + 2 >= tokens.Count) return false;

                Token nextToken = tokens[i + 1];
                if (!IsValidBindableProperty(nextToken)) return false;

                if (tokens[i + 2].Type.Equals(TokenType.CloseBrace))
                {
                    i += 2;
                    continue;
                }

                if (i + 4 >= tokens.Count) return false;
                if (!tokens[i + 2].Type.Equals(TokenType.Pipe)) return false;
                if (!IsValidArgument(tokens[i + 3])) return false;
                if (!tokens[i + 4].Type.Equals(TokenType.CloseBrace)) return false;

                i += 4;
                continue;
            }

            return false;
        }

        return true;
    }

    private bool IsValidBindableProperty(Token token)
    {
        return token.Type.Equals(TokenType.String) && BindablePropertyLookup.IsValidBindableProperty(token.Value);
    }

    private bool IsValidArgument(Token token)
    {
        return token.Type.Equals(TokenType.String) && int.TryParse(token.Value, out _);
    }
}
