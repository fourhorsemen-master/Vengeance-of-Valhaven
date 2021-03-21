using System;

[Serializable]
public class SceneDataLookup : SerializableEnumDictionary<Scene, SceneData>{
    public SceneDataLookup(SceneData defaultValue) : base(defaultValue) {}
    public SceneDataLookup(Func<SceneData> defaultValueProvider) : base(defaultValueProvider) {}
}
