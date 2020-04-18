using System.Collections.Generic;

public class TokenValidator
{
    public bool HasValidSyntax(List<Token> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            Token token = tokens[i];
            
            if (token.Type.Equals(TokenType.String)) continue;

            if (token.Type.Equals(TokenType.OpenBrace))
            {
                if (i + 2 >= tokens.Count ) return false;

                Token nextToken = tokens[i + 1];
                if (!IsValidBindableProperty(nextToken)) return false;

                Token nextNextToken = tokens[i + 2];
                if (!nextNextToken.Type.Equals(TokenType.CloseBrace)) return false;

                i += 2;
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
}
