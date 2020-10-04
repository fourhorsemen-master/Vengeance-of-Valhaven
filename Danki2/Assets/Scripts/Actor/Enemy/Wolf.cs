﻿using UnityEngine;

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

    [SerializeField]
    private float agroTime = 0f;

    [SerializeField]
    private BoxCollider boxCollider = null;

    [SerializeField]
    private GameObject meshGameObject = null;

    private float _biteRemainingCooldown = 0f;
    private float _pounceRemaningCooldown = 0f;

    private const float meshDisableOnDeathDelay = 2f;

    public AudioSource howl;

    private const float HowlRange = 10f;
    public override ActorType Type => ActorType.Wolf;
    public bool BiteOffCooldown => _biteRemainingCooldown <= 0f;
    public bool PounceOffCooldown => _pounceRemaningCooldown <= 0f;

    private Coroutine attentionCoroutine;

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

    public void GetAttention(Player player)
    {
        if (attentionCoroutine != null) return;

        MovementManager.StopPathfinding();
        MovementManager.Watch(player.transform);
        attentionCoroutine = this.WaitAndAct(agroTime, () =>
        {
            FindTarget(player);
        });
    }

    public void LoseAttention()
    {
        if (attentionCoroutine != null)
        {
            StopCoroutine(attentionCoroutine);
            attentionCoroutine = null;
        }
    }

    public void FindTarget(Player player)
    {
        if (attentionCoroutine != null)
        {
            StopCoroutine(attentionCoroutine);
        }
        
        Target = player;
        Howl();
        OnHowl.Next(this);
        MusicManager.Instance.StartCombatMusic();
    }

    private void Howl()
    {
        howl.Play();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        this.WaitAndAct(meshDisableOnDeathDelay, () =>
        {
            boxCollider.enabled = false;
            DisableNavMeshAgent();
            meshGameObject.SetActive(false);
        });        
    }
}
