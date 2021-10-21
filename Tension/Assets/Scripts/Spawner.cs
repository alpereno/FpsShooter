using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private Enemy enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    Vector3 randomSpawnPos;


    private void Start()
    {
        nextWave();
    }

    private void Update()
    {
        spawnEnemy();
    }

    private void spawnEnemy()
    {
        //control number of enemies to spawn with a certain time
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            randomSpawnPos = createRandomPosBetweenChilds();
            Enemy spawnedEnemy = Instantiate(enemy, randomSpawnPos, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += onEnemyDeath;
        }
    }

    void onEnemyDeath() {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            nextWave();
        }
    }

    void nextWave() {
        currentWaveNumber++;
        print("Wave: " + currentWaveNumber);
        if (currentWaveNumber-1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    Vector3 createRandomPosBetweenChilds(){
        int childNumber = transform.childCount;
        int randomChild = Random.Range(0, childNumber);
        Vector3 randomPos = transform.GetChild(randomChild).transform.position;
        return randomPos;
    }    

    [System.Serializable]
    public class Wave{
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
