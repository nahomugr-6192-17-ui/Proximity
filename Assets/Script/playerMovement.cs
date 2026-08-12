using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public LineRenderer aimLine;
    public float pushForce = 4f;
    public float stopSpeed = 0.1f;
    public bool isMoving = false;
    public bool isTouchingPlayer = false;
    public Vector2 startTouchPosition;
    public Vector2 dragVector;
    public Vector2 shotDirection;
    public float shotDistance;
    private int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Debug.Log("STARTED: " + gameObject.name +
          " | Score: " + score);
        rb.bodyType = RigidbodyType2D.Static;
        aimLine.positionCount = 2;
        aimLine.enabled = false;
        isMoving = false;
        isTouchingPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Touchscreen.current == null)
        {
            return;
        }

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
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
                aimLine.enabled = true;

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

            shotDistance = dragVector.magnitude;

            shotDirection = -dragVector.normalized;

            Vector2 aimLineStart = (Vector2)transform.position;
            Vector2 aimLineEnd = aimLineStart + shotDirection * shotDistance;
            aimLine.SetPosition(0, aimLineStart);
            aimLine.SetPosition(1, aimLineEnd);
        }
        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame && isTouchingPlayer)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(shotDirection * shotDistance * pushForce, ForceMode2D.Impulse);
            isTouchingPlayer = false;
            isMoving = true;
            aimLine.enabled = false;
        }

        if (rb.linearVelocity.magnitude < stopSpeed && isMoving)
        {
            Debug.Log("RED PLAYER STOPPED!");
            isMoving = false;
            string finalZone = FinalZone();
            CalculateScore(finalZone);
        }
    }

    public string FinalZone()
    {
        Collider2D[] zones = Physics2D.OverlapPointAll(transform.position);

        foreach (Collider2D zone in zones)
        {
            switch (zone.gameObject.tag)
            {
                case "80":
                    return "80";

                case "60":
                    return "60";

                case "40":
                    return "40";

                case "20":
                    return "20";

                case "Survival":
                    return "Survival";
            }
        }
        return "Unknown";
    }

    public void CalculateScore(string finalZone)
    {
        switch (finalZone)
        {
            case "80":
                score += 80;
                break;

            case "60":
                score += 60;
                break;

            case "40":
                score += 40;
                break;

            case "20":
                score += 20;
                break;

            case "Survival":
                score += 0;
                break;
            case "Unknown":
                score -= 50;
                break;
        }

        Debug.Log("SCORE: " + gameObject.name +
          " | Current Score: " + score);
        Debug.Log("Final Zone: " + finalZone);
    }
}
