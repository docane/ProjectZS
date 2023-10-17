using UnityEngine;

public class ZombieController : LivingEntity
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Dead
    }
    public int startingHealth;
    State currentState = State.Idle;
    EnemyAni myAni;

    Transform playerPos;
    PlayerMovementScript player;

    float chaseDistance = 50.0f;
    public float attackDistance = 2.0f;
    float reChaseDistance = 3.0f;

    float rotAnglePerSecond = 360f;

    public float moveSpeed = 1.0f;
    public float attackDelay = 1.0f;
    float attackTimer = 0.0f;
    public float damage = 13;

    bool killed = true;

    public AudioSource _idleSound1;
    public AudioSource _idleSound2;
    public AudioSource _deadSound;

    float soundTimer = 0.0f;
    float soundDelay = 3.0f;

    public float animSpeed = 1;

    public GameObject mediKit;
    public GameObject ammoBox;
    public int medikitProbability = 3;
    public int ammoboxProbability = 25;
    bool dropItem = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        health = startingHealth;
        dead = false;
        myAni = GetComponent<EnemyAni>();
        ChangeState(State.Idle, EnemyAni.IDLE);
        playerPos = GameObject.Find("Player").transform;
        player = GameObject.Find("Player").GetComponent<PlayerMovementScript>();
        GetComponent<Animator>().SetFloat("walkSpeed", animSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        if (health <= 0 && killed)
        {
            killed = false;
            player.zombieKill += 1;
        }
        Sound();
    }

    public void ChangeState(State newState, string aniName)
    {
        if (currentState == newState)
        {
            return;
        }
        currentState = newState;
        myAni.ChangeAni(aniName);
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Dead:
                DeadState();
                break;
        }
    }

    void IdleState()
    {
        if (GetDistanceFromPlayer() < chaseDistance)
        {
            ChangeState(State.Chase, EnemyAni.WALK);
        }

        if (health <= 0)
        {
            ChangeState(State.Dead, EnemyAni.DIE);
        }
    }

    void ChaseState()
    {
        
        if (health <= 0)
        {
            ChangeState(State.Dead, EnemyAni.DIE);
        }
        if (GetDistanceFromPlayer() < attackDistance)
        {
            ChangeState(State.Attack, EnemyAni.ATTACK);
        }
        if (GetDistanceFromPlayer() > chaseDistance)
        {
            ChangeState(State.Idle, EnemyAni.IDLE);
        }
        else
        {
            TurnToDestination();
            MoveToDestination();
        }
    }

    void AttackState()
    {
        if (health <= 0)
        {
            ChangeState(State.Dead, EnemyAni.DIE);
        }
        if (GetDistanceFromPlayer() > reChaseDistance)
        {
            attackTimer = 0.0f;
            ChangeState(State.Chase, EnemyAni.WALK);
        }
        else
        {
            if (attackTimer > attackDelay)
            {
                Vector3 temp = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
                transform.LookAt(temp);
                myAni.ChangeAni(EnemyAni.ATTACK);
                player.TakeHit(damage);
                attackTimer = 0.0f;
            }
        }
        attackTimer += Time.deltaTime;
    }

    void DeadState()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        if (!dropItem)
        {
            DropItem();
        }
        Destroy(this.gameObject, 5f);
    }

    void DropItem()
    {
        int random = Random.Range(0, 100);
        if (random < medikitProbability)
        {
            Instantiate(mediKit, transform.position, transform.rotation);
        }
        else if (random - medikitProbability < ammoboxProbability)
        {
            Instantiate(ammoBox, transform.position, transform.rotation);
        }
        dropItem = true;
    }

    void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(playerPos.position.x - transform.position.x, 0, playerPos.position.z - transform.position.z));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);
    }

    float GetDistanceFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);
        return distance;
    }

    void Sound()
    {
        soundTimer += Time.deltaTime;
        if (soundTimer >= soundDelay && !dead)
        {
            int temp = Random.Range(1, 3);
            if (temp == 1)
                _idleSound1.Play();
            else
                _idleSound2.Play();
            soundTimer = 0;
        }
        if (dead)
        {
            _idleSound1.Stop();
            _idleSound2.Stop();
            _deadSound.Play();
        }
    }
}