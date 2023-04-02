using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject wallPrefab;
    public int mapRows = 2;
    public int mapColumns = 2;
    public float tileSize;
    public float wallHeight = 1f;
    public bool generateBorderWall = true;
    public int numOfCoins;
    public GameObject coin;

    private void Start()
    {
        if (UpgradeManager.mapUpgrade1) {
            mapRows = 5;
            mapColumns = 5;
        }
        if (UpgradeManager.mapUpgrade2) {
            mapRows = 10;
            mapColumns = 10;
        }
        GenerateMap();
        GenerateCoins();

        if (generateBorderWall)
        {
            GenerateBorderWall();
        }
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

    private void GenerateBorderWall()
    {
        float wallSize = 4.0f;
        int wallUnitsPerTile = Mathf.CeilToInt(tileSize / wallSize);

        for (int x = -wallUnitsPerTile; x < (mapRows * wallUnitsPerTile) - wallUnitsPerTile + 1; x++)
        {
            for (int z = -wallUnitsPerTile; z < (mapColumns * wallUnitsPerTile) - wallUnitsPerTile + 1; z++)
            {
                if (x == -wallUnitsPerTile || z == -wallUnitsPerTile || x == (mapRows * wallUnitsPerTile) - wallUnitsPerTile || z == (mapColumns * wallUnitsPerTile) - wallUnitsPerTile)
                {
                    for (int y = 0; y < wallHeight; y++)
                    {
                        Vector3 position = new Vector3((x * wallSize) + (tileSize / 2), (y * wallSize) + (wallSize / 2), (z * wallSize) + (tileSize / 2));
                        Quaternion rotation = Quaternion.identity;
                        GameObject wallInstance = Instantiate(wallPrefab, position, rotation);
                        wallInstance.transform.parent = transform;
                    }
                }
            }
        }
    }

    public void GenerateCoins() {
        for (int i = 1; i < numOfCoins; i++) {
            float coinX = Random.Range(-15, tileSize * mapRows - 21);
            float coinZ = Random.Range(-15, tileSize * mapColumns - 21);
            Quaternion rotation = Quaternion.identity;

            GameObject coinGen = Instantiate(coin, new Vector3(coinX, 0.7f, coinZ), rotation);
        }
    }
}
