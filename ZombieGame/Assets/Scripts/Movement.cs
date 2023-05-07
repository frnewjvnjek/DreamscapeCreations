using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 500f;
    public float jumpDelay;

    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 moveDirection;

    public LayerMask groundLayer;
    private bool isGrounded;
    private bool isSprinting;
    private bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        canJump = true;
    }

    void Update()
    {
        // Get player input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Calculate movement direction
        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = (verticalInput * cameraForward + horizontalInput * mainCamera.transform.right).normalized;

        // Apply movement
        if (moveDirection != Vector3.zero)
        {
            float speed = isSprinting ? sprintSpeed : walkSpeed;
            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
            canJump = false;
            Invoke(nameof(ResetJump), jumpDelay);
        }
    }

    void FixedUpdate()
    {
        // Check if player is grounded
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1f, groundLayer))
        {
            isGrounded = true;
            Debug.DrawRay(ray.origin, Vector3.down, Color.red);
        }
    }

    void ResetJump()
    {
        canJump = true;
    }
}
