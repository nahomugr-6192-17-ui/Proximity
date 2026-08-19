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
        if (collision.gameObject.CompareTag("Coin"))
        {
            Debug.Log(
                owner.name + " coin collided with " +
                collision.gameObject.name
            );
        }
    }
}