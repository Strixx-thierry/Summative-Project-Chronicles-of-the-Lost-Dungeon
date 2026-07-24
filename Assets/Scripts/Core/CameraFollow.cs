using UnityEngine;
using UnityEngine.InputSystem;

// Third-person camera that orbits behind the player with the mouse
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 4.5f;
    [SerializeField] private float height = 1.9f;
    [SerializeField] private float sensitivity = 0.18f;
    [SerializeField] private float smoothing = 12f;

    private float yaw;
    private float pitch = 12f;

    void Start()
    {
        // Self-heal: grab the player if the reference was lost
        if (target == null)
        {
            var player = GameObject.Find("Player");
            if (player != null) target = player.transform;
        }
        if (target != null) yaw = target.eulerAngles.y;
        LockCursor(true);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Free the cursor while the game is paused, recapture when playing
        bool paused = GameManager.Instance != null && GameManager.Instance.State != GameManager.GameState.Playing;
        LockCursor(!paused);
        if (paused) { PositionCamera(); return; }

        var mouse = Mouse.current;
        if (mouse != null)
        {
            Vector2 delta = mouse.delta.ReadValue();
            yaw += delta.x * sensitivity;
            pitch = Mathf.Clamp(pitch - delta.y * sensitivity, -5f, 55f);
        }
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.rightStick.ReadValue();
            yaw += stick.x * sensitivity * 12f;
            pitch = Mathf.Clamp(pitch - stick.y * sensitivity * 8f, -5f, 55f);
        }

        PositionCamera();
    }

    void PositionCamera()
    {

        var rotation = Quaternion.Euler(pitch, yaw, 0);
        var focus = target.position + Vector3.up * height;
        var wanted = focus - rotation * Vector3.forward * distance;

        // Pull in if a wall is between camera and player
        if (Physics.Linecast(focus, wanted, out var hit, ~0, QueryTriggerInteraction.Ignore))
            wanted = hit.point + hit.normal * 0.25f;

        transform.position = Vector3.Lerp(transform.position, wanted, smoothing * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(focus - transform.position), smoothing * Time.deltaTime);
    }

    static void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    public float Yaw => yaw;

    // Movement uses the camera's facing so forward is always away from the camera
    public Vector3 FlatForward
    {
        get
        {
            var f = transform.forward;
            f.y = 0;
            return f.sqrMagnitude < 0.001f ? Vector3.forward : f.normalized;
        }
    }

    public Vector3 FlatRight
    {
        get
        {
            var r = transform.right;
            r.y = 0;
            return r.sqrMagnitude < 0.001f ? Vector3.right : r.normalized;
        }
    }
}
