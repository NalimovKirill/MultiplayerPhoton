using UnityEngine;

public class Coin : MonoBehaviour
{
    private int _priceOfCoin = 1; // Points from taking 1 coin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController enemy = collision.GetComponent<PlayerController>();

        if (enemy != null)
        {
            enemy.CollectCoin(_priceOfCoin);
            Destroy(gameObject);
        }
    }
}
