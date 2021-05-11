using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class RuneTooltipBuilderTest
{
    [UnityTest]
    public IEnumerator TestRuneToolTipsCanAllBeBuilt()
    {
        TestUtils.InstantiatePrefab<RuneLookup>();

        yield return null;

        EnumUtils.ForEach<Rune>(rune =>
        {
            Assert.DoesNotThrow(() =>
            {
                RuneTooltipBuilder.Build(rune);
            }, $"{rune} cannot be built.");
        });

        RuneLookup.Instance.Destroy();

        yield return null;
    }
}