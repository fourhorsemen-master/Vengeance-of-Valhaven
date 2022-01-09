using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class AbilityLookupTest : PlayModeTestBase
{
    protected override IEnumerator SetUp()
    {
        yield return base.SetUp();
        TestUtils.InstantiatePrefab<AbilityLookup>();
        TestUtils.InstantiatePrefab<AbilityTypeLookup>();
        yield return null;
    }

    protected override IEnumerator TearDown()
    {
        AbilityLookup.Instance.Destroy();
        AbilityTypeLookup.Instance.Destroy();
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestAbilityLookupIds()
    {
        AbilityLookup.Instance.ForEachAbilityId(abilityId =>
        {
            string displayName = AbilityLookup.Instance.GetDisplayName(abilityId);
            Assert.True(AbilityLookup.Instance.TryGetAbilityId(displayName, out SerializableGuid _));
            AbilityLookup.Instance.GetAbilityType(abilityId);
            AbilityLookup.Instance.GetDamage(abilityId);
            AbilityLookup.Instance.GetEmpowerments(abilityId);
            AbilityLookup.Instance.GetRarity(abilityId);
            AbilityLookup.Instance.GetIcon(abilityId);
        });
        
        yield return null;
    }
}
