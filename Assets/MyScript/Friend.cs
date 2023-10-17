using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : LivingEntity
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    State currentState = State.Idle;
    FriendAni myAni;

    Transform playerPos;
    PlayerMovementScript player;

    GameObject[] enemies;
    GameObject enemy;

    float chaseDistance = 10000.0f;
    float attackDistance = 15.0f;
    float reChaseDistance = 8.0f;
    float rotAnglePerSecond = 360f;

    float moveSpeed = 5.0f;
    float attackDelay = 1.5f;
    float attackTimer = 0.0f;
    float damage = 33;

    public GameObject[] muzzelFlash;
    public GameObject muzzelSpawn;
    public AudioSource shoot_sound_source;
    private GameObject holdFlash;
    public GameObject bullet;
    RaycastHit hit;
    public GameObject bloodEffect;
    public LayerMask ignoreLayer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dead = false;
        myAni = GetComponent<FriendAni>();
        ChangeState(State.Idle, FriendAni.IDLE);
        playerPos = GameObject.Find("Player").transform;
        player = GameObject.Find("Player").GetComponent<PlayerMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GetDistanceFromEnemies();
        UpdateState();
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
            ChangeState(State.Chase, FriendAni.WALK);
        }
        if (GetDistanceFromEnemy() < attackDistance)
        {
            ChangeState(State.Attack, FriendAni.ATTACK);
        }
        if (health <= 0)
        {
            ChangeState(State.Dead, FriendAni.DIE);
        }
    }

    void ChaseState()
    {
        if (health <= 0)
        {
            ChangeState(State.Dead, FriendAni.DIE);
        }
        if (GetDistanceFromPlayer() > 5.0f)
        {
            TurnToDestination();
            MoveToDestination();
        }
        else if (GetDistanceFromEnemy() < attackDistance)
        {
            ChangeState(State.Attack, FriendAni.ATTACK);
        }
        else
        {
            ChangeState(State.Idle, FriendAni.IDLE);
        }
    }

    void AttackState()
    {
        if (health <= 0)
        {
            ChangeState(State.Dead, FriendAni.DIE);
        }
        if (GetDistanceFromEnemy() > attackDistance)
        {
            ChangeState(State.Idle, FriendAni.IDLE);
        }
        TurnToEnemy();
        if (GetDistanceFromPlayer() < reChaseDistance)
        {
            if (attackTimer > attackDelay)
            {
                myAni.ChangeAni(FriendAni.ATTACK);

                int randomNumberForMuzzelFlash = Random.Range(0, 5);
                Instantiate(bullet, muzzelSpawn.transform.position, muzzelSpawn.transform.rotation);
                holdFlash = Instantiate(muzzelFlash[randomNumberForMuzzelFlash], muzzelSpawn.transform.position /*- muzzelPosition*/, muzzelSpawn.transform.rotation * Quaternion.Euler(0, 0, 90)) as GameObject;
                holdFlash.transform.parent = muzzelSpawn.transform;
                shoot_sound_source.Play();
                enemy.GetComponent<ZombieController>().TakeHit(damage);
                attackTimer = 0.0f;
            }
        }
        if (GetDistanceFromPlayer() > reChaseDistance)
        {
            attackTimer = 0.0f;
            ChangeState(State.Chase, FriendAni.WALK);
        }
        attackTimer += Time.deltaTime;
    }

    void DeadState()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(this.gameObject, 5f);
    }

    void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(playerPos.position.x - transform.position.x, 0, playerPos.position.z - transform.position.z));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    void TurnToEnemy()
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(enemy.transform.position.x - transform.position.x, 0, enemy.transform.position.z - transform.position.z));

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

    void GetDistanceFromEnemies()
    {
        float distance = 9999;
        foreach (GameObject enemy in enemies)
        {
            bool dead = enemy.GetComponent<ZombieController>().dead;
            float temp = Vector3.Distance(transform.position, enemy.transform.position);
            if (temp < distance && !dead)
            {
                distance = temp;
                this.enemy = enemy;
            }
        }
    }

    float GetDistanceFromEnemy()
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        return distance;
    }
}
