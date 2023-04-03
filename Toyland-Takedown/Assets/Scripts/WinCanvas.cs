using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvas : MonoBehaviour
{
    public GameObject winUI;
    public float timeToMainMenu;

    void Update()
    {
        if (EnemyCount.enemyCount <= 0) {
            winUI.SetActive(true);
            timeToMainMenu -= Time.deltaTime;

            if (timeToMainMenu <= 0) {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
