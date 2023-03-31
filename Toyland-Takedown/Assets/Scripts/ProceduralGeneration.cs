using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public int mapRows;
    public int mapColumns;
    public float tileSize;

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapRows; x++)
        {
            for (int z = 0; z < mapColumns; z++)
            {
                GameObject tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tileInstance = Instantiate(tilePrefab, position, Quaternion.Euler(0, 90 * Random.Range(0, 4), 0));
                tileInstance.transform.parent = transform;
            }
        }
    }
}
