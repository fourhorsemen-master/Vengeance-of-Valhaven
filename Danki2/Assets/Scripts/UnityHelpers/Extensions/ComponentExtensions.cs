using UnityEngine;

public static class ComponentExtensions
{
    /// <summary>
    /// Returns true if the supplied tag matches the gameObject's tag
    /// </summary>
    public static bool CompareTag(this Component component, Tag tag)
    {
        return component.CompareTag(tag.ToString());
    }
}