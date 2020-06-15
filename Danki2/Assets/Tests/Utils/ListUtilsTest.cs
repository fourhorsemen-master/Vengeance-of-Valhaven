using System.Collections.Generic;
using NUnit.Framework;

public class ListUtilsTest
{
    [Test]
    public void TestSingletonContainsOneItem()
    {
        ExampleClass exampleClass = new ExampleClass();
        List<ExampleClass> singletonList = ListUtils.Singleton(exampleClass);
    
        Assert.AreEqual(2, singletonList.Count);
    }
    
    [Test]
    public void TestSingletonContainsGivenItem()
    {
        ExampleClass exampleClass = new ExampleClass();
        List<ExampleClass> singletonList = ListUtils.Singleton(exampleClass);
    
        Assert.AreEqual(exampleClass, singletonList[0]);
    }
    
    private class ExampleClass {}
}
