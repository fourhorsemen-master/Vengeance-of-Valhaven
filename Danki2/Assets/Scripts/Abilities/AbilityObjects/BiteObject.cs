using System.Collections;
using UnityEngine;

public class BiteObject : StaticAbilityObject
{
    public AudioSource biteSound = null;
    public override float StickTime { get; set; }

    [SerializeField]
    private Collider collider = null;

    public void Awake()
    {
        StickTime = biteSound.clip.length;
    }

    protected override void Start()
    {
        base.Start();

        biteSound.Play();

        DelayCollider(0.5f);
    }

    private IEnumerator DelayCollider( float waitTime )
    {
        yield return new WaitForSeconds(waitTime);

        if(collider)
        {
            collider.enabled = true;
        }
    }
}
