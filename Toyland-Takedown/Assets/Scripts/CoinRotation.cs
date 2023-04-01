using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    void Start() {
        gameObject.transform.Rotate(new Vector3(90, 0, 0));
    }

    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, 0.5f));
    }
}
