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
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * 100f * Time.deltaTime
        );
    }

    public void RotateToPlayer(
        playerMovement player,
        bool instant = false)
    {
        Vector3 targetPosition = transform.position;

        if (player.playerName == "Red")
        {
            targetPosition = new Vector3(0f, -10.3f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (player.playerName == "Blue")
        {
            targetPosition = new Vector3(-10.3f, 0f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 270f);
        }
        else if (player.playerName == "Green")
        {
            targetPosition = new Vector3(0f, 10.3f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (player.playerName == "Purple")
        {
            targetPosition = new Vector3(10.3f, 0f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 90f);
        }

        transform.position = targetPosition;

        if (instant)
        {
            transform.rotation = targetRotation;
        }
    }
}