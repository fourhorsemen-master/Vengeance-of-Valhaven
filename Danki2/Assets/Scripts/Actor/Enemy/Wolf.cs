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

    private float biteRemainingCooldown = 0f;
    private float pounceRemainingCooldown = 0f;

    public AudioSource howl;

    private const float HowlRange = 10f;
    public override ActorType Type => ActorType.Wolf;
    public bool BiteOffCooldown => biteRemainingCooldown <= 0f;
    public bool PounceOffCooldown => pounceRemainingCooldown <= 0f;

    public Subject<Wolf> OnHowl { get; private set; } = new Subject<Wolf>();

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
            biteRemainingCooldown -= Time.deltaTime;
            if (biteRemainingCooldown <= 0f)
            {
                biteRemainingCooldown = 0f;
            }
        }

        if (!PounceOffCooldown)
        {
            pounceRemainingCooldown -= Time.deltaTime;
            if (pounceRemainingCooldown <= 0f)
            {
                pounceRemainingCooldown = 0f;
            }
        }
    }

    public void Bite()
    {
        if (biteRemainingCooldown > 0)
        {
            return;
        }

        WaitAndCast(_biteCastTime, new Bite(), Target.transform.position);
        biteRemainingCooldown = _biteTotalCooldown;
    }

    public void Pounce()
    {
        if (pounceRemainingCooldown > 0)
        {
            return;
        }

        WaitAndCast(_pounceCastTime, new Pounce(), Target.transform.position);
        biteRemainingCooldown = _biteTotalCooldown;
        pounceRemainingCooldown = _pounceTotalCooldown;
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
}
