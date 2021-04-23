using UnityEditor;
using UnityEngine;

public static class TestUtils
{
    public static T InstantiatePrefab<T>(string assetPath) where T : Object
    {
        T prefab = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        return Object.Instantiate(prefab);
    }
}
