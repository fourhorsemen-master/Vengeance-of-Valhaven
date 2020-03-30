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
        Initialize(NoOpType, new float[0]);
    }

    public SerializableAIElement(Type type, float[] args)
    {
        Initialize(type, args);
    }

    public void OnAfterDeserialize()
    {
        Initialize(_serializedType.Type, _serializedArgs);
    }

    public void OnBeforeSerialize()
    {
        _serializedType = new SerializableType(AiElement.GetType());
        _serializedArgs = AiElement.Args;
    }

    private void Initialize(Type type, float[] args)
    {
        AiElement = (T)Activator.CreateInstance(type);
        AiElement.Args = args;
    }
}
