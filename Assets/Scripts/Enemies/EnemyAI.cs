using UnityEngine;

// Melee enemy driven by a State machine: idle, chase, attack, dead
[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float detectRange = 12f;
    [SerializeField] private float attackRange = 2.2f;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float turnSpeed = 8f;
    [SerializeField] private int attackDamage = 15;
    [SerializeField] private float attackCooldown = 1.6f;
    [SerializeField] private Transform model;
    [SerializeField] private float modelYawOffset = 180f;
    [SerializeField] private EnemyBehaviourType behaviourType = EnemyBehaviourType.Simple;

    public Transform Player { get; private set; }
    public Animator Animator { get; private set; }
    public float DetectRange => detectRange;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public bool Busy { get; private set; }

    private Rigidbody rb;
    private IEnemyState state;
    private IEnemyBehaviour behaviour;
    private bool dead;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (model == null) model = transform;
        Animator = GetComponentInChildren<Animator>();
        behaviour = CreateBehaviour(behaviourType);
        var health = GetComponent<Health>();
        if (health != null) health.OnDied += OnDied;
    }

    IEnemyBehaviour CreateBehaviour(EnemyBehaviourType type) => type switch
    {
        EnemyBehaviourType.Ram => new RamBehaviour(),
        EnemyBehaviourType.JumpSmash => new JumpSmashBehaviour(),        
        EnemyBehaviourType.PhaseStrike => new PhaseStrikeBehaviour(),

        _ => new SimpleBehaviour(),
    };

    // Runs the current behaviour's attack sequence; Busy gates the state machine meanwhile
    public void StartAttack()
    {
        if (!Busy) StartCoroutine(RunAttack());
    }

    System.Collections.IEnumerator RunAttack()
    {
        Busy = true;
        yield return behaviour.Attack(this);
        Busy = false;
    }

    // ---- helpers used by behaviours ----
    public int AttackDamage => attackDamage;
    public Vector3 DirectionToPlayer() =>
        Player != null ? (Flat(Player.position) - Flat(transform.position)).normalized : transform.forward;
    public Vector3 PlayerPosition(float fallbackAhead) =>
        Player != null ? Player.position : transform.position + transform.forward * fallbackAhead;
    public void PlayAnim(string trigger) { if (Animator != null) Animator.SetTrigger(trigger); }
    public void SetPosition(Vector3 pos) => transform.position = pos;
    public void ChargeToward(Vector3 dir, float speed)
    {
        dir.y = 0; dir.Normalize();
        rb.linearVelocity = new Vector3(dir.x * speed, rb.linearVelocity.y, dir.z * speed);
        FaceDirectionPublic(dir);
    }
    public void DamageInRadius(float radius)
    {
        if (Player == null) return;
        if (Vector3.Distance(Flat(transform.position), Flat(Player.position)) <= radius)
            Player.GetComponent<Health>()?.TakeDamage(attackDamage);
    }
    public void FaceDirectionPublic(Vector3 dir) => FaceDirection(dir);

    void Start()
    {
        Player = FindFirstObjectByType<PlayerController>()?.transform;
        SetState(new EnemyIdleState());
    }

    void Update()
    {
        if (dead) return;
        state?.Tick(this);
    }

    public void SetState(IEnemyState next)
    {
        state = next;
        next.Enter(this);
    }

    public float DistanceToPlayer() =>
        Player == null ? 999f : Vector3.Distance(Flat(transform.position), Flat(Player.position));

    public void MoveToward(Vector3 target)
    {
        Vector3 dir = Flat(target) - Flat(transform.position);
        if (dir.sqrMagnitude > 0.01f)
        {
            Vector3 step = dir.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(step.x, rb.linearVelocity.y, step.z);
            FaceDirection(dir);
        }
        SetAnimSpeed(moveSpeed);
    }

    public void StopMoving()
    {
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        SetAnimSpeed(0);
    }

    public void FacePlayer()
    {
        if (Player != null) FaceDirection(Player.position - transform.position);
    }

    void FaceDirection(Vector3 dir)
    {
        dir.y = 0;
        if (model == null || dir.sqrMagnitude < 0.01f) return;
        Quaternion look = Quaternion.LookRotation(dir) * Quaternion.Euler(0, modelYawOffset, 0);
        model.rotation = Quaternion.Slerp(model.rotation, look, turnSpeed * Time.deltaTime);
    }

    public void TriggerAttack()
    {
        if (Animator != null) Animator.SetTrigger("Attack");
    }

    public void TriggerTaunt()
    {
        if (Animator != null) Animator.SetTrigger("Taunt");
    }

    // Called at the swing; only lands if the player is still in range
    public void TryDamagePlayer()
    {
        if (Player == null || DistanceToPlayer() > attackRange + 0.7f) return;
        Player.GetComponent<Health>()?.TakeDamage(attackDamage);
    }

    void SetAnimSpeed(float s)
    {
        if (Animator != null) Animator.SetFloat("Speed", s);
    }

    void OnDied()
    {
        dead = true;
        StopMoving();
        if (Animator != null) Animator.SetTrigger("Dead");
    }

    static Vector3 Flat(Vector3 v) => new Vector3(v.x, 0, v.z);
}
