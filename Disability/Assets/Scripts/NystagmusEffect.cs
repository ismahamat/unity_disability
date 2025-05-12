using UnityEngine;

public class NystagmusZone : MonoBehaviour
{
    [Header("Nystagmus Settings")]
    public Camera playerCamera;
    public float frequency = 15f;
    public float amplitude = 1.5f;
    public float recoverySpeed = 3f;
    public Vector2 verticalLimits = new Vector2(-2f, 2f);

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isInZone = false;

    void Start()
    {
        if (playerCamera != null)
        {
            originalPosition = playerCamera.transform.localPosition;
            originalRotation = playerCamera.transform.localRotation;
        }
        else
        {
            Debug.LogError("Player Camera is not assigned!");
        }
    }

    void Update()
    {
        if (isInZone && playerCamera != null)
        {
            float xOffset = Mathf.Sin(Time.time * frequency) * amplitude;
            float yOffset = Mathf.Sin(Time.time * frequency * 0.5f) * amplitude * 0.5f;
            yOffset = Mathf.Clamp(yOffset, verticalLimits.x, verticalLimits.y);

            Vector3 newPosition = originalPosition + new Vector3(xOffset, yOffset, 0);
            Quaternion newRotation = Quaternion.Euler(new Vector3(yOffset * 5f, xOffset * 5f, 0));

            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, newPosition, Time.deltaTime * recoverySpeed);
            playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, newRotation, Time.deltaTime * recoverySpeed);
        }
        else if (playerCamera != null)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalPosition, Time.deltaTime * recoverySpeed);
            playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, originalRotation, Time.deltaTime * recoverySpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = false;
        }
    }
}
