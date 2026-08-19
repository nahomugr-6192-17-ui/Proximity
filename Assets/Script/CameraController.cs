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
        if (player.playerName == "Red")
        {
            transform.position = new Vector3(0f, -9.8f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (player.playerName == "Blue")
        {
            transform.position = new Vector3(-9.8f, 0f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 270f);
        }
        else if (player.playerName == "Green")
        {
            transform.position = new Vector3(0f, 9.8f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (player.playerName == "Purple")
        {
            transform.position = new Vector3(9.8f, 0f, -10f);
            targetRotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }
}