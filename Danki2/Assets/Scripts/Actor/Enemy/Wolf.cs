using UnityEngine;

public class Wolf : Enemy
{
    public AudioSource howl;

    public override ActorType Type => ActorType.Wolf;
    public bool biteOffCooldown => _biteRemainingCooldown <= 0f;
    public bool pounceOffCooldown => _pounceRemaningCooldown <= 0f;

    [SerializeField]
    private float _biteTotalCooldown;
    [SerializeField]
    private float _pounceTotalCooldown;

    private float _biteRemainingCooldown = 0f;
    private float _pounceRemaningCooldown = 0f;

    public void Howl()
    {
        howl.Play();
    }

    protected override void Update()
    {
        base.Update();

        if (!biteOffCooldown)
        {
            _biteRemainingCooldown -= Time.deltaTime;
            if (_biteRemainingCooldown <= 0f)
            {
                _biteRemainingCooldown = 0f;
            }
        }

        if (!pounceOffCooldown)
        {
            _pounceRemaningCooldown -= Time.deltaTime;
            if (_pounceRemaningCooldown <= 0f)
            {
                _pounceRemaningCooldown = 0f;
            }
        }
    }

    public void Bite()
    {
        new Bite(new AbilityContext(this, Target.transform.position)).Cast();
        _biteRemainingCooldown += _biteTotalCooldown;
    }

    public void Pounce()
    {
        StartChannel(
            new Pounce(
                new AbilityContext(
                    this,
                    Target.transform.position
                )
            )
        );
        _biteRemainingCooldown += _biteTotalCooldown;
        _pounceRemaningCooldown += _pounceTotalCooldown;
    }

    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 2;
        transform.Rotate(Vector3.forward, 90f);
    }
}
