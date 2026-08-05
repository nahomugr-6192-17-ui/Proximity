using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityY = speed;
    }
}
