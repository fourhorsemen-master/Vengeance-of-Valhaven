using System.Collections.Generic;

public class Lexer
{
    private const char OpenBrace = '{';
    private const char CloseBrace = '}';
    private const char Pipe = '|';

    /// <summary>
    /// Turns a tooltip into a list of tokens. A token represents a distinct part of our tooltip templating
    /// language, for example, and open brace "{". The Lexer does not care about the syntax being correct,
    /// but only about reading the string into a more specific raw representation.
    /// </summary>
    /// <param name="tooltip"> The tooltip to lex. </param>
    /// <returns> The list of tokens. </returns>
    public List<Token> Lex(string tooltip)
    {
        List<Token> tokens = new List<Token>();

        string currentString = "";
        foreach (char c in tooltip)
        {
            if (IsString(c))
            {
                currentString += c;
                continue;
            }

            if (!currentString.Equals(""))
            {
                tokens.Add(new Token(TokenType.String, currentString));
                currentString = "";
            }

            if (IsBrace(c))
            {
                tokens.Add(new Token(GetBraceType(c), c.ToString()));
            }

            if (IsPipe(c))
            {
                tokens.Add(new Token(TokenType.Pipe, c.ToString()));
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
        return !IsBrace(c) && !IsPipe(c);
    }

    private bool IsBrace(char c)
    {
        return c.Equals(OpenBrace) || c.Equals(CloseBrace);
    }

    private TokenType GetBraceType(char brace)
    {
        return brace.Equals(OpenBrace) ? TokenType.OpenBrace : TokenType.CloseBrace;
    }

    private bool IsPipe(char c)
    {
        return c.Equals(Pipe);
    }
}
