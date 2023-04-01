using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : MonoBehaviour
{
    public float timeToDestroy;

    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0) {
            Destroy(gameObject);
        }
    }
}
