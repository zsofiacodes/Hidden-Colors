using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float lookSensitivity = 0.1f;

    [Header("Jump & Ground")]
    [SerializeField] private float jumpStrength = 7f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float cameraPitch = 0f;
    private bool isRunning;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        cameraPitch -= lookInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -30f, 70f);
        cameraTarget.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        if (animator != null)
        {
            float currentHorizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
            animator.SetFloat("Speed", currentHorizontalSpeed / runSpeed);
            animator.SetBool("Grounded", isGrounded);
            animator.SetBool("Falling", !isGrounded && rb.linearVelocity.y < -0.1f);
        }
    }

    private void FixedUpdate()
    {
        bool canRun = isRunning && moveInput.y > 0.1f;
        float currentSpeed = canRun ? runSpeed : walkSpeed;

        Vector3 movement = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
        movement = movement.normalized * currentSpeed;

        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();

    public void OnSprint(InputValue value) => isRunning = value.isPressed;

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            if (animator != null) animator.SetTrigger("Jump");
        }
    }
}