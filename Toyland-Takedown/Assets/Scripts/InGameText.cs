using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameText : MonoBehaviour
{
    public TMP_Text gemCountText;

    void Update()
    {
        gemCountText.text = "GEMS: " + GemCount.gemCount;
    }
}
