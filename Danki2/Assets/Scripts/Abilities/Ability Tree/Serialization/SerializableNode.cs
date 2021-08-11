using System;
using UnityEngine;

[Serializable]
public class SerializableNode
{
    [SerializeField] private Ability2 ability;
    [SerializeField] private int id;
    [SerializeField] private int leftChildId;
    [SerializeField] private int rightChildId;
    
    public Ability2 Ability => ability;
    public int Id => id;
    public int LeftChildId => leftChildId;
    public int RightChildId => rightChildId;
    
    public SerializableNode(Ability2 ability, int id, int leftChildId, int rightChildId)
    {
        this.ability = ability;
        this.id = id;
        this.leftChildId = leftChildId;
        this.rightChildId = rightChildId;
    }
}
