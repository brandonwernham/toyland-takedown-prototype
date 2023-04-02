using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    public AudioClip coinSound;

    void Start()
    {
        CoinCount.coinsThisGame = 0;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            CoinCount.coinCount += 1;
            CoinCount.coinsThisGame += 1;
            Debug.Log("Coins: " + CoinCount.coinCount);
            Debug.Log("Coins this game: " + CoinCount.coinsThisGame);
            Destroy(gameObject);
        }
    }
}
