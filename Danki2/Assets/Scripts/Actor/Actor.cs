using Assets.Scripts.AI;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public abstract AI AI { get; }

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        this.Act();
    }

    public virtual void Act()
    {
        this.AI.Act();
    }
}
