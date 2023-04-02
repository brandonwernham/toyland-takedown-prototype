using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public string mainMenu;
    public Button mu1;
    public Button mu2;
    public Button gu1;
    public Button gu2;
    public AudioSource soundManager;
    public AudioClip hoverSound;
    public AudioClip purchaseSound;
    public TMP_Text coincountText;

    void Update()
    {
        coincountText.text = "COINS: " + CoinCount.coinCount;

        if (CoinCount.coinCount < 10)
        {
            mu1.interactable = false;
            gu1.interactable = false;
        }

        if (CoinCount.coinCount < 30)
        {
            mu2.interactable = false;
            gu2.interactable = false;
        }

        if (UpgradeManager.mapUpgrade1)
        {
            mu1.GetComponent<Image>().color = Color.green;
            mu1.interactable = false;
        }
        if (UpgradeManager.mapUpgrade2)
        {
            mu2.GetComponent<Image>().color = Color.green;
            mu2.interactable = false;
        }
        if (UpgradeManager.gemUpgrade1)
        {
            gu1.GetComponent<Image>().color = Color.green;
            gu1.interactable = false;
        }
        if (UpgradeManager.gemUpgrade2)
        {
            gu2.GetComponent<Image>().color = Color.green;
            gu2.interactable = false;
        }
    }

    public void HoverSound()
    {
        soundManager.PlayOneShot(hoverSound);
    }

    public void PurchaseSound()
    {
        if (gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().interactable)
        {
            soundManager.PlayOneShot(purchaseSound);
        }
    }

    public void MU1()
    {
        PurchaseSound();
        UpgradeManager.mapUpgrade1 = true;
        CoinCount.coinCount -= 10;
    }

    public void MU2()
    {
        PurchaseSound();
        UpgradeManager.mapUpgrade2 = true;
        CoinCount.coinCount -= 30;
    }

    public void GU1()
    {
        PurchaseSound();
        UpgradeManager.gemUpgrade1 = true;
        CoinCount.coinCount -= 10;
    }

    public void GU2()
    {
        PurchaseSound();
        UpgradeManager.gemUpgrade2 = true;
        CoinCount.coinCount -= 30;
    }

    public void Back()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
