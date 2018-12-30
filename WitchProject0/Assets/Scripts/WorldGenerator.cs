using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//old world generator, ton of tweeking to do
//https://www.youtube.com/watch?v=xNozfY_Iah8&list=PL4NBTD2Gfy0hwx3m_dogvMydIDb4HiXyP

public class WorldGenerator : MonoBehaviour
{

    public GameObject player;

    public int sizeX = 10;
    public int sizeZ = 10;

    public float terDetail = 1;
    public float terHeight = 1;
    int seed;

    public GameObject[] blocks;

    // Use this for initialization
    void Start()
    {
        seed = Random.Range(100000, 999999);
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //créer le premier chunk de terrain au lancement du jeu
    void GenerateTerrain()
    {
        for (int x = 0; x < sizeX; x += 2)
        {
            for (int z = 0; z < sizeZ; z += 2)
            {
                //première couche qui détermine la randomness
                int maxY = (int)(Mathf.PerlinNoise((x + seed) / terDetail, (z + seed) / terDetail) * terHeight);

                if (maxY > 3)
                {
                    maxY = 3;
                }

                //empêche de placer quelque chose sur la bedrock
                if (maxY == 0)
                {
                    maxY = 1;
                }

                //pose l'herbe sur le dessus
               
                GameObject grass = Instantiate(blocks[0], new Vector3(x, maxY, z), Quaternion.identity) as GameObject;
                grass.transform.SetParent(GameObject.FindGameObjectWithTag("Environment").transform);

                //pose l'eau sur tout le reste de la couche du bas
                /*for (float y = 0f; y < 0.5f; y += 0.5f)
                {
                    GameObject water = Instantiate(blocks[2], new Vector3(x, y, z), Quaternion.identity) as GameObject;
                    water.transform.SetParent(GameObject.FindGameObjectWithTag("Environment").transform);
                }*/

                //pose la terre en-dessous
                /*for (float y = 0.5f; y < maxY; y += 0.5f)
                {
                    GameObject dirt = Instantiate(blocks[1], new Vector3(x, y, z), Quaternion.identity) as GameObject;
                    dirt.transform.SetParent(GameObject.FindGameObjectWithTag("Environment").transform);
                }*/

                //place le joueur au milieu du terrain
                /*if (x == (int)(sizeX / 2) && z == (int)(sizeZ / 2))
                {
                    Instantiate(player, new Vector3(x, maxY + 3, z), Quaternion.identity);
                }*/
            }
        }
    }
}
