// No unnecessary using statements
using UnityEngine;

// Don't use namespaces
/// <summary>
/// This file acts as a styleguide, showing naming conventions and the correct ordering of items within a class.
/// Within each item, different privacy levels appear in this order: Public, Protected, Private.
/// </summary>
public class Example : MonoBehaviour
{
    // Fields
    private int field;

    // Properties
    public int Property { get; set; }

    // Constructor
    public Example()
    {    
        // Never use var keyword
        int exampleInt = 5;

        // Use 'this.' for clarity when using private fields
        this.field = exampleInt;
    }

    // Mono-behaviour methods
    private void Start()
    {        
    }

    // Static methods
    public static void StaticMethod()
    {

    }

    // Methods
    public void Method()
    {

    }

    // Private classes
    private class SubExample
    {
    }
}

// We only allow 1 thing (Class, Interface, Enum) per file, but make an exception for generics.
public class Example<T>
{

}
