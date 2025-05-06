using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // D�placement souris (cam�ra)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // D�placement clavier
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // gravit�
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
