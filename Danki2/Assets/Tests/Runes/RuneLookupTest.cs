using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

public class RuneLookupTest
{
    private readonly Lexer lexer = new Lexer();
    private readonly TokenValidator tokenValidator = new TokenValidator();

    [UnityTest]
    public IEnumerator TestRuneToolTipsHaveValidSyntax()
    {
        TestUtils.InstantiatePrefab<RuneLookup>();

        EnumUtils.ForEach<Rune>(rune =>
        {
            List<Token> tokens = lexer.Lex(RuneLookup.Instance.GetTooltip(rune));
            Assert.True(tokenValidator.HasValidSyntax(tokens), $"{rune} has invalid syntax.");
        });

        RuneLookup.Instance.Destroy();

        yield return null;
    }
}
