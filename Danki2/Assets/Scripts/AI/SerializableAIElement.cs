using System;
using UnityEngine;

public abstract class SerializableAIElement<T> : ISerializationCallbackReceiver where T : AIElement
{
    public T AiElement { get; set; }

    [SerializeField]
    private SerializableType _serializedType;

    [SerializeField]
    private float[] _serializedArgs;

    protected abstract Type NoOpType { get; }

    public SerializableAIElement()
    {
        DeseralizeArgs(NoOpType, new float[0]);
    }

    public SerializableAIElement(Type type, float[] args)
    {
        DeseralizeArgs(type, args);
    }

    public void OnAfterDeserialize()
    {
        DeseralizeArgs(_serializedType.Type, _serializedArgs);
    }

    public void OnBeforeSerialize()
    {
        _serializedType = new SerializableType(AiElement.GetType());
        _serializedArgs = AiElement.Args;
    }

    private void DeseralizeArgs(Type type, float[] args)
    {
        AiElement = (T)Activator.CreateInstance(type);
        AiElement.Args = args;
    }
}
