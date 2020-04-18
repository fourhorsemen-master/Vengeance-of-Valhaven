using System.Collections.Generic;

public class Lexer
{
    private const char OpenBrace = '{';
    private const char CloseBrace = '}';

    private readonly string test = "Deals {DAMAGE} damage to targets in a cone.";
    
    public List<Token> Lex(string tooltip)
    {
        List<Token> tokens = new List<Token>();

        string currentString = "";
        foreach (char c in tooltip)
        {
            if (IsString(c))
            {
                currentString += c;
            }
            else if (IsBrace(c))
            {
                if (!currentString.Equals(""))
                {
                    tokens.Add(new Token(TokenType.String, currentString));
                    currentString = "";
                }

                tokens.Add(new Token(GetBraceType(c), c.ToString()));
            }
        }

        if (!currentString.Equals(""))
        {
            tokens.Add(new Token(TokenType.String, currentString));
        }

        return tokens;
    }

    private bool IsString(char c)
    {
        return !IsBrace(c);
    }

    private bool IsBrace(char c)
    {
        return c.Equals(OpenBrace) || c.Equals(CloseBrace);
    }

    private TokenType GetBraceType(char brace)
    {
        return brace.Equals(OpenBrace) ? TokenType.OpenBrace : TokenType.CloseBrace;
    }
}
