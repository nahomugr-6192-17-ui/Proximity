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
    public bool canPlay = false;
    public Vector2 startTouchPosition;
    public Vector2 dragVector;
    public Vector2 shotDirection;
    public float shotDistance;
    private int[] turnScores = new int[3];

    private int totalScore;
    public int TotalScore => totalScore;

    public int CurrentRoundScore
    {
        get
        {
            int total = 0;

            for (int i = 0; i < turnScores.Length; i++)
            {
                total += turnScores[i];
            }

            return total;
        }
    }
    public goldenCoin goldCoinScript;
    public Vector2 startPosition;
    public string playerName;
    public GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // scoreText.text = "Score: " + score;
        rb.bodyType = RigidbodyType2D.Static;
        aimLine.positionCount = 2;
        aimLine.enabled = false;
        isMoving = false;
        isTouchingPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay)
        {
            return;
        }
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
            PlayerStopped();
        }
    }

    public void PlayerStopped()
    {
        isMoving = false;
        canPlay = false;
        gameManager.PlayerFinishedTurn(this);
    }

    public int CalculateCurrentScore()
    {
        string finalZone = FinalZone();

        bool isGoldCoinSafe =
            goldCoinScript.IsGoldCoinSafe();

        bool isCollidedWithPlayer =
            goldCoinScript.hasCollidedWithPlayer;

        if (isGoldCoinSafe &&
            isCollidedWithPlayer &&
            finalZone == "80")
        {
            return GetZoneScore("100");
        }

        if (
            (isGoldCoinSafe &&
             isCollidedWithPlayer &&
             finalZone != "80") ||

            (isGoldCoinSafe &&
             !isCollidedWithPlayer)
        )
        {
            return GetZoneScore(finalZone);
        }

        if (
            (isCollidedWithPlayer &&
             !isGoldCoinSafe) ||

            finalZone == "Unknown"
        )
        {
            return GetZoneScore("Negative");
        }

        return 0;
    }

    public void StartPlayerTurn()
    {
        canPlay = true;
        rb.bodyType = RigidbodyType2D.Static;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        isMoving = false;
        isTouchingPlayer = false;
    }

    public void ResetCoin()
    {
        rb.position = startPosition;
        rb.bodyType = RigidbodyType2D.Static;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        isMoving = false;
        isTouchingPlayer = false;
    }

    public void FinalizeRoundScore()
    {
        totalScore += CurrentRoundScore;

        ResetTurnScores();
    }

    public void ResetTurnScores()
    {
        for (int i = 0; i < turnScores.Length; i++)
        {
            turnScores[i] = 0;
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

    public int GetZoneScore(string finalZone)
    {
        switch (finalZone)
        {
            case "100":
                return 100;

            case "80":
                return 80;

            case "60":
                return 60;

            case "40":
                return 40;

            case "20":
                return 20;

            case "Survival":
                return 0;

            case "Unknown":
                return -50;

            case "Negative":
                return -30;
        }

        return 0;
    }

    public void SetTurnScore(int turnIndex, int newScore)
    {
        if (turnIndex < 0 || turnIndex >= turnScores.Length)
        {
            return;
        }

        turnScores[turnIndex] = newScore;
    }

    public int GetTurnScore(int turnIndex)
    {
        if (turnIndex < 0 || turnIndex >= turnScores.Length)
        {
            return 0;
        }

        return turnScores[turnIndex];
    }
}
