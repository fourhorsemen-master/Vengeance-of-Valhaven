using UnityEngine;

public class Wolf : Enemy
{
    [SerializeField]
    private float _biteCastTime = 0f;
    [SerializeField]
    private float _pounceCastTime = 0f;

    [SerializeField]
    private float _biteTotalCooldown = 0f;
    [SerializeField]
    private float _pounceTotalCooldown = 0f;

    private float _biteRemainingCooldown = 0f;
    private float _pounceRemaningCooldown = 0f;

    public AudioSource howl;

    private const float HowlRange = 10f;

    public override ActorType Type => ActorType.Wolf;

    public bool BiteOffCooldown => _biteRemainingCooldown <= 0f;
    public bool PounceOffCooldown => _pounceRemaningCooldown <= 0f;

    public Subject<Wolf> OnHowl { get; } = new Subject<Wolf>();
    public WolfPack Pack { get; private set; }

    private bool HasPack => Pack != null;

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

                if (distance < HowlRange)
                {
                    Join(other);
                    Target = other.Target;
                    this.WaitAndAct(0.5f, Howl);
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
        if (_biteRemainingCooldown > 0)
        {
            return;
        }

        WaitAndCast(
            _biteCastTime,
            AbilityReference.Bite,
            () => transform.position + transform.forward
        );
        
        _biteRemainingCooldown = _biteTotalCooldown;
    }

    public void Pounce()
    {
        if (_pounceRemaningCooldown > 0)
        {
            return;
        }

        WaitAndCast(
            _pounceCastTime,
            AbilityReference.Pounce,
            () => Target.transform.position + (transform.position - Target.transform.position).normalized
        );
        
        _biteRemainingCooldown = _biteTotalCooldown;
        _pounceRemaningCooldown = _pounceTotalCooldown;
    }

    public void CallFriends(Player player)
    {
        Target = player;
        Howl();
        OnHowl.Next(this);
    }

    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 2;
        transform.Rotate(Vector3.forward, 90f);
    }

    private void Howl()
    {
        howl.Play();
    }

    private void Join(Wolf other)
    {
        if (HasPack && other.HasPack)
        {
            other.Pack.Add(this);
            Pack = other.Pack;
        }
        else if (HasPack && !other.HasPack)
        {
            Pack.Add(other);
        }
        else if (!HasPack && other.HasPack)
        {
            other.Pack.Add(this);
        }
        else
        {
            WolfPack pack = new WolfPack(this, other);
            Pack = pack;
            other.Pack = pack;
        }
    }
}
