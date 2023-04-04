using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvas : MonoBehaviour
{
    public GameObject winUI;
    public float timeToMainMenu;
    public AudioSource audioSource;
    public AudioClip winSound;
    int timesToPlay = 1;

    void Update()
    {
        if (EnemyCount.enemyCount <= 0) {
            if (timesToPlay > 0) {
                audioSource.PlayOneShot(winSound);
                timesToPlay--;
            }
            
            winUI.SetActive(true);
            timeToMainMenu -= Time.deltaTime;

            if (timeToMainMenu <= 0) {
                CoinCount.coinCount += CoinCount.coinsThisGame;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
