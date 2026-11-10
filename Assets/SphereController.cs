using UnityEngine;
using UnityEngine.InputSystem;

public class SphereController : MonoBehaviour
{
    [SerializeField] private float dragGain = 0.02f;
    [SerializeField] private float keyboardForce = 30f; // new: force for WASD
    [SerializeField] private Rigidbody rigidBody;

    private Vector2 previousPosition;
    private Vector3 pendingImpulse;

    void Awake()
    {
        if (rigidBody == null)
            rigidBody = GetComponentInChildren<Rigidbody>();

        if (rigidBody == null)
        {
            Debug.LogError("Assign the sphere's Rigidbody.", this);
            enabled = false;
        }
    }

    void Update()
    {
        // new: keyboard control (WASD), camera-relative
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            Vector2 keys = Vector2.zero;
            if (keyboard.wKey.isPressed) keys.y += 1f;
            if (keyboard.sKey.isPressed) keys.y -= 1f;
            if (keyboard.dKey.isPressed) keys.x += 1f;
            if (keyboard.aKey.isPressed) keys.x -= 1f;

            if (keys != Vector2.zero)
                pendingImpulse +=
                    ScreenDeltaToWorld(keys.normalized) * keyboardForce * Time.deltaTime;
        }

        // mouse / touch control (unchanged)
        var pointer = Pointer.current;
        if (pointer == null) return;

        Vector2 position = pointer.position.ReadValue();

        if (pointer.press.wasPressedThisFrame)
            previousPosition = position;
        else if (pointer.press.isPressed)
        {
            Vector2 delta = position - previousPosition;
            pendingImpulse +=
                ScreenDeltaToWorld(delta) * dragGain;
            previousPosition = position;
        }
    }

    // Converts a screen direction into a horizontal direction relative to the camera
    Vector3 ScreenDeltaToWorld(Vector2 delta)
    {
        Camera cam = Camera.main;
        if (cam == null)
            return new Vector3(delta.x, 0f, delta.y); // fallback: original behaviour

        Vector3 right = cam.transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 forward = Vector3.Cross(right, Vector3.up);

        return right * delta.x + forward * delta.y;
    }

    void FixedUpdate()
    {
        rigidBody.AddForce(pendingImpulse, ForceMode.Impulse);
        pendingImpulse = Vector3.zero;
    }
}