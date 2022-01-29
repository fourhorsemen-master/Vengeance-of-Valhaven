using System;
using UnityEngine;

[Serializable]
public class SerializableNode
{
    [SerializeField] private bool isRoot;
    [SerializeField] private SerializableAbility ability;
    [SerializeField] private int id;
    [SerializeField] private int leftChildId;
    [SerializeField] private int rightChildId;

    public bool IsRoot => isRoot;
    public SerializableAbility Ability => ability;
    public int Id => id;
    public int LeftChildId => leftChildId;
    public int RightChildId => rightChildId;
    
    public SerializableNode(bool isRoot, Ability ability, int id, int leftChildId, int rightChildId)
    {
        this.isRoot = isRoot;
        this.ability = isRoot ? null : ability.Serialize();
        this.id = id;
        this.leftChildId = leftChildId;
        this.rightChildId = rightChildId;
    }
}
