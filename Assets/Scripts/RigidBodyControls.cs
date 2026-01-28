using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidBodyControls_NewInput : MonoBehaviour
{
    [Header("Input Actions (Player map)")]
    [SerializeField] private InputActionReference move;

    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference lockCursor;
    [SerializeField] private InputActionReference unlockCursor;

    [Header("Movement")]
    public float moveSpeed = 7f;

    public float turnSpeed = 100f; // mouse yaw speed (deg/sec)

    [Header("Jump")]
    public float jumpPower = 6f;

    private Rigidbody rb;
    private float yaw;
    private Vector3 inputWorld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        yaw = transform.eulerAngles.y;
    }

    private void OnEnable()
    {
        move?.action.Enable();
        look?.action.Enable();
        jump?.action.Enable();
        jump.action.performed += OnJump;

        lockCursor.action.Enable();
        unlockCursor.action.Enable();

        lockCursor.action.performed += OnLock;
        unlockCursor.action.performed += OnUnlock;
    }

    private void OnDisable()
    {
        if (jump != null) jump.action.performed -= OnJump;
        move?.action.Disable();
        look?.action.Disable();
        jump?.action.Disable();

        lockCursor.action.performed -= OnLock;
        unlockCursor.action.performed -= OnUnlock;

        lockCursor.action.Disable();
        unlockCursor.action.Disable();
    }

    private void OnLock(InputAction.CallbackContext _)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnUnlock(InputAction.CallbackContext _)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        // Move action (WASD)
        Vector2 m = move.action.ReadValue<Vector2>();
        Vector3 local = new Vector3(m.x, 0f, m.y);
        inputWorld = transform.TransformDirection(local.normalized);

        // Look action (mouse)
        Vector2 delta = look.action.ReadValue<Vector2>();
        yaw += delta.x * turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputWorld * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnJump(InputAction.CallbackContext _)
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
    }
}