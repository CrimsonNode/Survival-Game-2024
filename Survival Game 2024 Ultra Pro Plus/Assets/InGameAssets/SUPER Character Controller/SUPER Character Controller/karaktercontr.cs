using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı
    public float jumpForce = 10f; // Zıplama kuvveti
    public float sprintSpeed = 10f; // Koşma hızı
    public float crouchSpeed = 2.5f; // Çömelmek hızı

    private Rigidbody rb;
    private bool isGrounded;
    private bool isCrouching;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Yürüme ve koşma
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = moveDirection * sprintSpeed;
        }
        else if (isCrouching)
        {
            rb.velocity = moveDirection * crouchSpeed;
        }
        else
        {
            rb.velocity = moveDirection * moveSpeed;
        }

        // Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Çömelmek
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                transform.localScale = new Vector3(1f, 0.5f, 1f); // Oyuncuyu küçült
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Oyuncuyu normal boyuta getir
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Yere temas etme kontrolü
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Yerden ayrılma kontrolü
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
