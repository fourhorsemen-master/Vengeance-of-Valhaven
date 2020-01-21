using System;
using UnityEngine;

public abstract class SerializableAIElement<T> : ISerializationCallbackReceiver where T : AIElement
{
    public T aiElement;

    protected abstract Type NoOpType { get; }

    [SerializeField]
    private SerializableType _serializedType;

    [SerializeField]
    private float[] _serializedArgs;

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
        _serializedType = new SerializableType(aiElement.GetType());
        _serializedArgs = aiElement.Args;
    }

    private void Initialize(Type type, float[] args)
    {
        aiElement = (T)Activator.CreateInstance(type);
        aiElement.Args = args;
    }
}
