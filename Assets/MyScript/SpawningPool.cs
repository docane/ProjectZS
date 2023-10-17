using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    int MaxZombieCount = 100;
    int keepMonsterCount = 0;
    Vector3 playerPos;
    Vector3 spawnPos;
    float spawnRadius = 5.0f;
    float spawnTime = 6.0f;
    float spawnTimer;
    float spawnDistance = 10;
    public GameObject zombie;
    public GameObject zombie2;
    public GameObject zombie3;
    public List<GameObject> zombies = new List<GameObject>();
    float Timer = 0;
    float difficulty = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        spawnTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        difficulty = (int)(Timer / 60);
        float randomX = Random.Range(spawnPos.x - spawnRadius, spawnPos.x + spawnRadius);
        float randomZ = Random.Range(spawnPos.z - spawnRadius, spawnPos.z + spawnRadius);
        playerPos = GameObject.Find("Player").transform.position;
        spawnTimer += Time.deltaTime;
        if (zombies.Count < MaxZombieCount)
        {
            if (spawnTimer > spawnTime + difficulty / 2)
            {
                if (spawnDistance < GetDistanceFromPlayer())
                {
                    spawnTimer = 0;
                    int random = Random.Range(0, 100);
                    if (random < 89-difficulty*2)
                    {
                        GameObject temp = Instantiate(zombie, new Vector3(randomX, spawnPos.y, randomZ), Quaternion.identity);
                        zombies.Add(temp);
                    }
                    else if (random < 97-difficulty)
                    {
                        GameObject temp = Instantiate(zombie2, new Vector3(randomX, spawnPos.y, randomZ), Quaternion.identity);
                        zombies.Add(temp);
                    }
                    else
                    {
                        GameObject temp = Instantiate(zombie3, new Vector3(randomX, spawnPos.y, randomZ), Quaternion.identity);
                        zombies.Add(temp);
                    }
                }
            }
        }
        foreach (GameObject zombie in zombies)
        {
            if (zombie.GetComponent<ZombieController>().dead)
            {
                zombies.Remove(zombie);
            }
        }
        Timer += Time.deltaTime;
    }

    float GetDistanceFromPlayer()
    {
        float distance = Vector3.Distance(playerPos, transform.position);
        return distance;
    }
}
