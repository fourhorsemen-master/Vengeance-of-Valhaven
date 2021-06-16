using System.Collections;
using UnityEngine.TestTools;

public abstract class PlayModeTestBase
{
    [UnitySetUp]
    protected virtual IEnumerator SetUp()
    {
        TestUtils.LoadEmptyScene();
        yield return null;
    }

    [UnityTearDown]
    protected virtual IEnumerator TearDown()
    {
        yield return null;
    }
}
