using Assets.Scripts.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public abstract IAI AI { get; }

    [SerializeField, Range(1f, 100f), Tooltip("Recommended value: 16.")]
    protected float moveSpeed = 1f;

    private Vector3 _vecToMove;

    // Start is called before the first frame update
    public void Start()
    {
        _vecToMove = new Vector3();
    }

    // Update is called once per frame
    public void Update()
    {
        this.Act();
    }

    protected virtual void Act()
    {
        this.AI.Act();
    }

    protected void MoveAlongVector(Vector3 vec)
    {
        vec.Normalize();
        vec *= moveSpeed;
        transform.Translate(vec);
    }

    protected void MoveToward(Vector3 target)
    {
        _vecToMove = target - transform.position;
        MoveAlongVector(_vecToMove);
    }

}
