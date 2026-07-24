using UnityEngine;
using UnityEngine.InputSystem;

// Fakes walk, run and attack motion by animating the model transform (no clips needed)
public class ProceduralAnimator : MonoBehaviour
{
    [SerializeField] private Transform model;
    [SerializeField] private Rigidbody body;

    [Header("Locomotion")]
    [SerializeField] private float bobHeight = 0.08f;
    [SerializeField] private float bobSpeed = 9f;
    [SerializeField] private float swayAngle = 5f;
    [SerializeField] private float runLean = 8f;

    [Header("Attack")]
    [SerializeField] private float attackDuration = 0.35f;
    [SerializeField] private float attackCooldown = 0.5f;

    private Vector3 baseLocalPos;
    private float walkPhase;
    private float attackTimer;
    private float cooldownTimer;
    private PlayerController controller;

    public bool IsAttacking => attackTimer > 0f;

    void Awake()
    {
        if (model == null && transform.childCount > 0) model = transform.GetChild(0);
        if (body == null) body = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
        if (model != null) baseLocalPos = model.localPosition;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            attackTimer = attackDuration;
            cooldownTimer = attackCooldown;
        }
    }

    void LateUpdate()
    {
        if (model == null) return;

        Vector3 vel = body != null ? body.linearVelocity : Vector3.zero;
        float planarSpeed = new Vector2(vel.x, vel.z).magnitude;
        float moveAmount = Mathf.Clamp01(planarSpeed / 6.5f);

        // Walk cycle: vertical bob plus a small roll, scaled by speed
        walkPhase += Time.deltaTime * bobSpeed * (0.5f + moveAmount);
        float bob = Mathf.Abs(Mathf.Sin(walkPhase)) * bobHeight * moveAmount;
        float sway = Mathf.Sin(walkPhase) * swayAngle * moveAmount;

        // Idle breathing when nearly still
        if (moveAmount < 0.05f)
            bob = Mathf.Sin(Time.time * 1.6f) * 0.015f;

        model.localPosition = baseLocalPos + Vector3.up * bob;

        float lean = moveAmount * runLean;
        Quaternion tilt = Quaternion.Euler(lean, 0f, sway);

        // Attack: quick forward swing that eases back
        Quaternion attack = Quaternion.identity;
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            float t = 1f - (attackTimer / attackDuration);
            float swing = Mathf.Sin(t * Mathf.PI);   // 0 -> 1 -> 0
            attack = Quaternion.Euler(-swing * 55f, 0f, 0f);
        }

        // Facing (from controller) composed with locomotion tilt and attack swing
        Quaternion facing = controller != null ? controller.ModelFacing : model.rotation;
        model.rotation = Quaternion.Slerp(model.rotation, facing * tilt * attack, 18f * Time.deltaTime);
    }
}
