using UnityEngine;

public class Wolf : Enemy
{
    public AudioSource howl;

    public override ActorType Type => ActorType.Wolf;
    public float howlRange = 10f;
    public bool BiteOffCooldown => _biteRemainingCooldown <= 0f;
    public bool PounceOffCooldown => _pounceRemaningCooldown <= 0f;

    public Observable<Wolf> OnHowl { get; private set; } = new Observable<Wolf>();

    [SerializeField]
    private float _biteTotalCooldown = 0f;
    [SerializeField]
    private float _pounceTotalCooldown = 0f;

    private float _biteRemainingCooldown = 0f;
    private float _pounceRemaningCooldown = 0f;

    protected override void Start()
    {
        base.Start();

        RoomManager.Instance.ActorCache.ForEach(actorCacheItem =>
        {
            if (actorCacheItem.Actor.Type != ActorType.Wolf) return;

            Wolf wolf = (Wolf)actorCacheItem.Actor;

            if (wolf.Equals(this)) return;

            wolf.OnHowl.Subscribe(other =>
            {
                float distance = Vector3.Distance(
                    transform.position,
                    other.transform.position
                );

                if (distance < howlRange)
                {
                    Target = other.Target;
                    Howl();
                }
            });
        });
    }

    protected override void Update()
    {
        base.Update();

        if (!BiteOffCooldown)
        {
            _biteRemainingCooldown -= Time.deltaTime;
            if (_biteRemainingCooldown <= 0f)
            {
                _biteRemainingCooldown = 0f;
            }
        }

        if (!PounceOffCooldown)
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

    public void CallFriends(Player player)
    {
        Target = player;
        Howl();
        OnHowl.Next(this);
    }

    private void Howl()
    {
        howl.Play();
    }

    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 2;
        transform.Rotate(Vector3.forward, 90f);
    }
}
