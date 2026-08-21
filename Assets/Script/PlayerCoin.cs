using UnityEngine;

public class playerCoin : MonoBehaviour
{
    public playerMovement owner;

    private void Start()
    {
        if (owner == null)
        {
            owner = GetComponentInParent<playerMovement>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        goldenCoin goldCoin =
            collision.gameObject.GetComponent<goldenCoin>();

        if (goldCoin != null)
        {
            bool directShot =
                owner.gameManager.CurrentPlayer == owner;

            goldCoin.SetLastPlayerToTouchGold(
                owner,
                directShot
            );

            Debug.Log(
                owner.playerName +
                " touched gold. Direct shot = " +
                directShot
            );

            return;
        }
    }
}