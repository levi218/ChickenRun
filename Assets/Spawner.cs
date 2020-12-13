using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] cats;
    public GameObject floor;
    public float rangeLimit;
    List<GameObject> obstacles;

    float timeTilNextSpawn;
    // Start is called before the first frame update
    void Start()
    {
        obstacles = new List<GameObject>();
        timeTilNextSpawn = 4;

    }

    void SpawnNewCat()
    {
        int level = GameController.Instance.point / 7;
        int maxCatType = level < cats.Length ? level : cats.Length;
        int catType = Random.Range(0, maxCatType);

        { 
            GameObject newGo = Instantiate<GameObject>(cats[catType], new Vector3(rangeLimit, floor.transform.position.y + 0.5f), Quaternion.identity);
            newGo.GetComponent<CatScript>().isActive = true;
            obstacles.Add(newGo);
        }
        if (level >= 5)
        {
            bool summonOneMore = Random.value > 0.05 * level;
            if (summonOneMore) { 
                GameObject newGo = Instantiate<GameObject>(cats[catType], new Vector3(rangeLimit+0.4f, floor.transform.position.y + 0.5f), Quaternion.identity);
                newGo.GetComponent<CatScript>().isActive = true;
                obstacles.Add(newGo);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = obstacles.Count-1;i>=0;i--)
        {
            if (obstacles[i].transform.position.x < -rangeLimit) {
                obstacles[i].GetComponent<CatScript>().isActive = false;
                GameObject.Destroy(obstacles[i]);
                obstacles.RemoveAt(i);
            } 
        }
        timeTilNextSpawn -= Time.deltaTime;
        if(timeTilNextSpawn < 0)
        {
            timeTilNextSpawn = Random.Range(2.5f, 3f);
            SpawnNewCat();
        }
    }
}
