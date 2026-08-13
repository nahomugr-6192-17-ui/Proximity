using UnityEngine;

public class goldenCoin : MonoBehaviour
{
    public Rigidbody2D rb;
    public playerMovement playerScript;
    public bool hasCollidedWithPlayer = false;

    public bool IsGoldCoinSafe()
    {
        Collider2D[] zones = Physics2D.OverlapPointAll(rb.position);

        foreach (Collider2D zone in zones)
        {
            if (zone.CompareTag("80"))
            {
                return true;
            }
        }
        return false;
    }

    public void ResetGoldCoinPosition(Vector2 newPosition, float newRotation)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.transform.position = newPosition;
        rb.rotation = newRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            hasCollidedWithPlayer = true;
            Debug.Log("Gold Coin collided with a player!");
        }
    }
}
