using UnityEngine;

public class goldenCoin : MonoBehaviour
{
    public Rigidbody2D rb;

    // The player who most recently caused the gold coin
    // to be touched.
    public playerMovement lastPlayerToTouchGold;
    public bool lastGoldTouchWasDirectShot;
    public playerMovement playerWhoKnockedOutGold;
    private Vector2 startPosition;
    private float startRotation;

    private void Start()
    {
        startPosition = rb.position;
        startRotation = rb.rotation;
    }

    public bool IsGoldCoinSafe()
    {
        Collider2D[] zones =
            Physics2D.OverlapPointAll(rb.position);

        foreach (Collider2D zone in zones)
        {
            if (zone.CompareTag("80"))
            {
                return true;
            }
        }

        return false;
    }

    public void SetLastPlayerToTouchGold(
        playerMovement player,
        bool directShot)
    {
        lastPlayerToTouchGold = player;

        lastGoldTouchWasDirectShot = directShot;

        Debug.Log(
            player.playerName +
            " is now the last player to touch gold. Direct Shot = " +
            directShot
        );
    }

    public void PlayerThatKnocksOutGoldCoin()
    {
        // Nobody touched the gold coin.
        if (lastPlayerToTouchGold == null)
        {
            return;
        }

        // Someone was already responsible.
        // Never change the penalty owner.
        if (playerWhoKnockedOutGold != null)
        {
            return;
        }

        // Gold is still safe.
        if (IsGoldCoinSafe())
        {
            return;
        }

        // Lock the last player who touched gold
        // at the exact moment gold became unsafe.
        playerWhoKnockedOutGold =
            lastPlayerToTouchGold;

        Debug.Log(
            playerWhoKnockedOutGold.playerName +
            " permanently receives -30 for this turn."
        );
    }

    public void ResetToStartPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.position = startPosition;
        rb.rotation = startRotation;

        lastPlayerToTouchGold = null;
        lastGoldTouchWasDirectShot = false;
        
    }
}