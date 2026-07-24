using UnityEngine;
using UnityEngine.InputSystem;

// Camera-relative movement with the model turning to face travel direction
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [SerializeField] private float turnSpeed = 14f;
    [SerializeField] private Transform model;
    [SerializeField] private float modelYawOffset = 0f;   // mesh orientation baked into the model pivot

    private Rigidbody rb;
    private CameraFollow cam;
    private Animator animator;
    private ProceduralAnimator proc;
    private Quaternion offset;

    // Direction the model should face; the procedural animator composes tilt on top of this
    public Quaternion ModelFacing { get; private set; } = Quaternion.identity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (model == null && transform.childCount > 0) model = transform.GetChild(0);
        animator = GetComponentInChildren<Animator>();
        proc = GetComponentInChildren<ProceduralAnimator>();
        offset = Quaternion.Euler(0, modelYawOffset, 0);
        ModelFacing = Quaternion.LookRotation(Vector3.forward) * offset;   // face into the room at start
    }

    void Start() => cam = Camera.main != null ? Camera.main.GetComponent<CameraFollow>() : null;

    void Update()
    {
        // Left click swings; the animator plays the attack state
        if (animator != null && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            animator.SetTrigger("Attack");
    }

    void FixedUpdate()
    {
        Vector2 input = ReadInput();
        bool sprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        float speed = sprinting ? sprintSpeed : walkSpeed;

        Vector3 forward = cam != null ? cam.FlatForward : Vector3.forward;
        Vector3 right = cam != null ? cam.FlatRight : Vector3.right;
        Vector3 move = (forward * input.y + right * input.x);
        if (move.sqrMagnitude > 1f) move.Normalize();

        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);

        if (move.sqrMagnitude > 0.01f)
            ModelFacing = Quaternion.Slerp(ModelFacing, Quaternion.LookRotation(move) * offset, turnSpeed * Time.fixedDeltaTime);

        // Apply facing here unless the procedural animator is active and owns the model
        bool procActive = proc != null && proc.enabled;
        if (!procActive && model != null)
            model.rotation = ModelFacing;

        // Feeds the animator once clips are imported
        if (animator != null)
            animator.SetFloat("Speed", move.magnitude * speed, 0.1f, Time.fixedDeltaTime);
    }

    static Vector2 ReadInput()
    {
        Vector2 input = Vector2.zero;
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) input.y += 1;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) input.y -= 1;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input.x += 1;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input.x -= 1;
        }
        if (Gamepad.current != null) input += Gamepad.current.leftStick.ReadValue();
        return Vector2.ClampMagnitude(input, 1f);
    }
}
