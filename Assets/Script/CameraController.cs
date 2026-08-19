using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform boardCenter;
    public float rotationSpeed = 5f;

    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void RotateToPlayer(playerMovement player)
    {
        Vector2 direction =
            player.transform.position - boardCenter.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Makes the current player appear at the bottom of the screen.
        float cameraAngle = angle + 90f;

        targetRotation =
            Quaternion.Euler(0f, 0f, cameraAngle);
    }
}