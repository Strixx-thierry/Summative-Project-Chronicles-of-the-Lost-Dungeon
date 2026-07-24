using UnityEngine;
using UnityEngine.InputSystem;

// WASD / left-stick movement across the dungeon floor
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
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
        input = Vector2.ClampMagnitude(input, 1f);

        rb.linearVelocity = new Vector3(input.x * moveSpeed, rb.linearVelocity.y, input.y * moveSpeed);
    }
}
