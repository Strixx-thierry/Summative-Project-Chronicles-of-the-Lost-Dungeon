using UnityEngine;
using UnityEngine.InputSystem;

// Third-person follow camera; orbits only while right mouse is held (stable otherwise)
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2.2f;
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private float smoothing = 12f;

    private float yaw;
    private float pitch = 18f;

    void Start()
    {
        if (target == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p == null) { var pc = FindFirstObjectByType<PlayerController>(); if (pc != null) p = pc.gameObject; }
            if (p != null) target = p.transform;
        }
        if (target != null) yaw = target.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        var mouse = Mouse.current;
        if (mouse != null && mouse.rightButton.isPressed)
        {
            Vector2 d = mouse.delta.ReadValue();
            yaw += d.x * sensitivity;
            pitch = Mathf.Clamp(pitch - d.y * sensitivity, -5f, 60f);
        }
        if (Gamepad.current != null)
        {
            Vector2 s = Gamepad.current.rightStick.ReadValue();
            if (s.magnitude > 0.15f)
            {
                yaw += s.x * sensitivity * 10f;
                pitch = Mathf.Clamp(pitch - s.y * sensitivity * 8f, -5f, 60f);
            }
        }

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 focus = target.position + Vector3.up * height;
        Vector3 wanted = focus - rot * Vector3.forward * distance;

        if (Physics.Linecast(focus, wanted, out var hit, ~0, QueryTriggerInteraction.Ignore))
            wanted = hit.point + hit.normal * 0.2f;

        transform.position = Vector3.Lerp(transform.position, wanted, smoothing * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(focus - transform.position), smoothing * Time.deltaTime);
    }

    public Vector3 FlatForward
    {
        get { var f = transform.forward; f.y = 0; return f.sqrMagnitude < 0.001f ? Vector3.forward : f.normalized; }
    }

    public Vector3 FlatRight
    {
        get { var r = transform.right; r.y = 0; return r.sqrMagnitude < 0.001f ? Vector3.right : r.normalized; }
    }
}
