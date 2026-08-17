using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private int totalScore;
    public Text scoreText;
    public int coinCounter = 3;
    public Text coinCounterText;
    public goldenCoin goldCoinScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        scoreText.text = "Score: " + score;
        coinCounterText.text = "Coins: " + coinCounter;
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
            if (coinCounter <= 0)
            {
                coinCounterText.text = "Coins: 0";
                return;
            }
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
            goldCoinScript.hasCollidedWithPlayer = false;
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
            bool isGoldCoinSafe = goldCoinScript.IsGoldCoinSafe();
            bool isCollidedWithPlayer = goldCoinScript.hasCollidedWithPlayer;
            
            /*Debug.Log("Final Zone: " + finalZone);
            Debug.Log("Is Gold Coin in 80: " + isGoldCoinSafe);
            Debug.Log("Collision with a player: " + isCollidedWithPlayer);*/
            
            if (isGoldCoinSafe && isCollidedWithPlayer && finalZone == "80")
            {
                CalculateScore("100");
            }
            else if ((isGoldCoinSafe && isCollidedWithPlayer && finalZone != "80") || (isGoldCoinSafe && !isCollidedWithPlayer))
            {
                CalculateScore(finalZone);
            }
            else if ((isCollidedWithPlayer && !isGoldCoinSafe) || finalZone == "Unknown")
            {
                CalculateScore("Negative");
                goldCoinScript.ResetGoldCoinPosition(new Vector2(0, 0), 0f);
                Debug.Log("Condition: Collided with player but gold coin is not in 80 zone. Gold coin reset to (0,0)");
            }

            rb.position = new Vector2(0, -20);
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
        score = 0;
        switch (finalZone)
        {
            case "100":
                score += 100;
                break;
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
            case "Negative":
                score -= 30;
                break;
        }

        totalScore += score;
        scoreText.text = "Score: " + totalScore;
        coinCounter--;
        coinCounterText.text = "Coins: " + coinCounter;
    }
}
