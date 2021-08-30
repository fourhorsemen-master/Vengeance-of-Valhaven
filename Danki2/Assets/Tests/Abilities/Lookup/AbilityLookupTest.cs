using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class AbilityLookupTest : PlayModeTestBase
{
    protected override IEnumerator SetUp()
    {
        yield return base.SetUp();
        TestUtils.InstantiatePrefab<AbilityLookup2>();
        yield return null;
    }

    protected override IEnumerator TearDown()
    {
        AbilityLookup2.Instance.Destroy();
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestAbilityLookupIds()
    {
        AbilityLookup2.Instance.ForEachAbilityId(abilityId =>
        {
            string displayName = AbilityLookup2.Instance.GetDisplayName(abilityId);
            Assert.True(AbilityLookup2.Instance.TryGetAbilityId(displayName, out SerializableGuid _));
            AbilityLookup2.Instance.GetAbilityType(abilityId);
            AbilityLookup2.Instance.GetDamage(abilityId);
            AbilityLookup2.Instance.GetEmpowerments(abilityId);
            AbilityLookup2.Instance.GetRarity(abilityId);
            AbilityLookup2.Instance.GetCollisionSoundLevel(abilityId);
            AbilityLookup2.Instance.GetIcon(abilityId);
        });
        
        yield return null;
    }
}
