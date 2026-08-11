using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float pushForce = 15f;
    public bool isMoving = false;
    public bool isTouchingPlayer = false;
    public Vector2 startTouchPosition;
    public Vector2 dragVector;
    public Vector2 shotDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.bodyType = RigidbodyType2D.Static;
        isMoving = false;
        isTouchingPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Touchscreen.current == null)
            return;

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            /*rb.bodyType = RigidbodyType2D.Dynamic;
            isMoving = true;*/
            Vector2 touchPosition =
                Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition =
                Camera.main.ScreenToWorldPoint(touchPosition);

            worldPosition.z = 0f;

            Collider2D hit =
                Physics2D.OverlapPoint(worldPosition);

            if (hit != null && hit.gameObject == gameObject)
            {
                startTouchPosition = worldPosition;
                isTouchingPlayer = true;

                Debug.Log("RED PLAYER TOUCHED!");
            }
            else
            {
                isTouchingPlayer = false;
                Debug.Log("RED PLAYER NOT TOUCHED!");
            }
        }
        if (Touchscreen.current.primaryTouch.press.isPressed && isTouchingPlayer)
        {
            Vector2 currentTouchPosition =
                Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 currentWorldPosition =
                Camera.main.ScreenToWorldPoint(currentTouchPosition);

            currentWorldPosition.z = 0f;

            dragVector =
                (Vector2)currentWorldPosition - startTouchPosition;

            Debug.Log("Drag Vector: " + dragVector);
        }
    }
}
